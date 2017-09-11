﻿using RateBlog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RateBlog.Models;
using RateBlog.Data;
using Microsoft.AspNetCore.Identity;
using RateBlog.Repository;

namespace RateBlog.Services
{
    public class SortService : ISortService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFeedbackService _feedbackService;
        private readonly IRepository<Platform> _platformRepo;
        private readonly IRepository<Category> _categoryRepo;
        private readonly IPlatformCategoryService _platformCategoryService;
        private readonly IRepository<Influencer> _influencerRepo;

        public SortService(IRepository<Influencer> influencerRepo, ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IFeedbackService feedbackService, IRepository<Platform> platformRepo, IRepository<Category> categoryRepo, IPlatformCategoryService platformCategoryService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _feedbackService = feedbackService;
            _platformRepo = platformRepo;
            _categoryRepo = categoryRepo;
            _platformCategoryService = platformCategoryService;
            _influencerRepo = influencerRepo; 
        }

       
        public async Task<IEnumerable<ApplicationUser>> SortInfluencer(string[] platforme, string[] kategorier, string search, int sortBy)
        {
            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }

            var influencer = from i in _influencerRepo.GetAll()
                             join user in _userManager.Users.ToList() on i.Id equals user.Id
                             select user;

            var influencers = influencer.Where(x => x.Name.ToLower().Contains(search.ToLower()) || _influencerRepo.Get(x.Id).Alias.Contains(search.ToLower())).ToList(); 

            foreach (var kategori in _categoryRepo.GetAll())
            {
                if (search.ToLower().Equals(kategori.Name.ToLower()))
                {
                    influencers.AddRange(await _platformCategoryService.GetAllInfluencersWithCategory(search.ToLower()));
                }
            }

            foreach (var platform in _platformRepo.GetAll())
            {
                if (search.ToLower().Equals(platform.Name.ToLower()))
                {
                    influencers.AddRange(await _platformCategoryService.GetAllInfluencersWithPlatform(search.ToLower()));
                }
            }

            if (platforme.Count() == 0 && kategorier.Count() == 0 && sortBy == 0)
            {
                return influencers;
            }
            else if (platforme.Count() == 0 && kategorier.Count() == 0 && sortBy != 0)
            {
                if (sortBy == 1)
                {
                    influencers = influencers.Select(x => new { user = x, score = _feedbackService.GetTotalScore(x.Id) }).OrderByDescending(o => o.score).Select(x => x.user).ToList();
                }
                else if (sortBy == 2)
                {
                    influencers = influencers.Select(x => new { user = x, score = _feedbackService.GetTotalScore(x.Id) }).OrderBy(o => o.score).Select(x => x.user).ToList();
                }
                else if (sortBy == 3)
                {
                    influencers = influencers.Select(x => new { user = x, score = _feedbackService.GetInfluencerFeedbackCount(x.Id) }).OrderByDescending(o => o.score).Select(x => x.user).ToList();
                }
                else if (sortBy == 4)
                {
                    influencers = influencers.Select(x => new { user = x, score = _feedbackService.GetInfluencerFeedbackCount(x.Id) }).OrderBy(o => o.score).Select(x => x.user).ToList();
                }
                return influencers;
            }

            var influencerCat = _dbContext.InfluencerCategory.Where(x => kategorier.Contains(x.CategoryId)).ToList();
            var influencerPlat = _dbContext.InfluencerPlatform.Where(x => platforme.Contains(x.PlatformId)).ToList();

            var cResult = influencerCat.GroupBy(x => x.InfluencerId).Where(p => p.Count() >= kategorier.Count()).SelectMany(x => x).ToList()/*.Select(x => x.InfluencerId).Distinct()*/.ToList();
            var pResult = influencerPlat.GroupBy(x => x.InfluencerId).Where(p => p.Count() >= platforme.Count()).SelectMany(x => x).ToList()/*.Select(x => x.InfluencerId).Distinct()*/.ToList();

            var ids = new List<string>();

            if (cResult.Count != 0 && pResult.Count != 0)
            {
                var result =
                    from cat in cResult
                    join plat in pResult on cat.InfluencerId equals plat.InfluencerId
                    select cat.InfluencerId;
                ids = result.ToList();
            }
            else if (pResult.Count != 0)
            {
                ids = pResult.Select(x => x.InfluencerId).ToList();
            }
            else if (cResult.Count != 0)
            {
                ids = cResult.Select(x => x.InfluencerId).ToList();
            }


            var list = influencers.Where(x => ids.Contains(x.Id)).ToList();

            if (sortBy == 1)
            {
                list = list.Select(x => new { user = x, score = _feedbackService.GetTotalScore(x.Id) }).OrderByDescending(o => o.score).Select(x => x.user).ToList();
            }
            else if (sortBy == 2)
            {
                list = list.Select(x => new { user = x, score = _feedbackService.GetTotalScore(x.Id) }).OrderBy(o => o.score).Select(x => x.user).ToList();
            }
            else if (sortBy == 3)
            {
                list = list.Select(x => new { user = x, score = _feedbackService.GetInfluencerFeedbackCount(x.Id) }).OrderByDescending(o => o.score).Select(x => x.user).ToList();
            }
            else if (sortBy == 4)
            {
                list = list.Select(x => new { user = x, score = _feedbackService.GetInfluencerFeedbackCount(x.Id) }).OrderBy(o => o.score).Select(x => x.user).ToList();
            }

            return list;
        }
    }
}