using RateBlog.Services.Interfaces;
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

        public SortService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IFeedbackService feedbackService, IRepository<Platform> platformRepo, IRepository<Category> categoryRepo, IPlatformCategoryService platformCategoryService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _feedbackService = feedbackService;
            _platformRepo = platformRepo;
            _categoryRepo = categoryRepo;
            _platformCategoryService = platformCategoryService;
        }

        public IEnumerable<ApplicationUser> InfluencerSortByPlatAndCat(int[] platformIds, int[] categoryIds, List<ApplicationUser> users)
        {
            if (platformIds.Count() == 0 && categoryIds.Count() == 0)
            {
                return users;
            }

            var influencerIds = new List<int>();
            var resultUserList = new List<ApplicationUser>();

            var resultCat = new List<InfluencerCategory>();
            var resultPlat = new List<InfluencerPlatform>();

            foreach (var v in users)
            {
                influencerIds.Add(v.InfluenterId.Value);
            }

            if (platformIds.Count() != 0)
            {
                var influenterPlatform = _dbContext.InfluencerPlatform.Where(x => platformIds.Contains(x.PlatformId) && influencerIds.Contains(x.InfluencerId)).ToList();
                resultPlat = influenterPlatform.GroupBy(x => x.InfluencerId).Where(p => p.Count() >= platformIds.Count()).SelectMany(x => x).ToList();

                foreach (var v in resultPlat)
                {
                    var user = _userManager.Users.SingleOrDefault(x => x.InfluenterId == v.InfluencerId);
                    if (!resultUserList.Contains(user))
                    {
                        resultUserList.Add(user);
                    }
                }
            }
            if (categoryIds.Count() != 0)
            {
                var influenterKategori = _dbContext.InfluencerCategory.Where(x => categoryIds.Contains(x.CategoryId) && influencerIds.Contains(x.InfluencerId)).ToList();
                resultCat = influenterKategori.GroupBy(x => x.InfluencerId).Where(p => p.Count() >= categoryIds.Count()).SelectMany(x => x).ToList();

                foreach (var v in resultCat)
                {
                    var user = _userManager.Users.SingleOrDefault(x => x.InfluenterId == v.InfluencerId);
                    if (!resultUserList.Contains(user))
                    {
                        resultUserList.Add(user);
                    }
                }
            }

            var endList = new List<ApplicationUser>();

            foreach (var user in resultUserList)
            {
                if (categoryIds.Count() == 0 || platformIds.Count() == 0)
                {
                    return resultUserList;
                }

                if (resultCat.Any(x => x.InfluencerId == user.InfluenterId) && resultPlat.Any(p => p.InfluencerId == user.InfluenterId))
                {
                    endList.Add(user);
                }
            }

            return endList;
        }

        public IEnumerable<ApplicationUser> SortInfluencer(int[] platforme, int[] kategorier, string search, int sortBy)
        {
            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }

            var influencers = _userManager.Users.
                Where(x => (x.Name.ToLower().Contains(search.ToLower())) && x.InfluenterId.HasValue
                || (x.Influenter.Alias.Contains(search) && x.InfluenterId.HasValue)).ToList();

            foreach (var kategori in _categoryRepo.GetAll())
            {
                if (search.ToLower().Equals(kategori.Name.ToLower()))
                {
                    influencers.AddRange(_platformCategoryService.GetAllInfluencersWithCategory(search));
                }
            }

            foreach (var platform in _platformRepo.GetAll())
            {
                if (search.ToLower().Equals(platform.Name.ToLower()))
                {
                    influencers.AddRange(_platformCategoryService.GetAllInfluencersWithPlatform(search));
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
                    influencers = influencers.Select(x => new { user = x, score = _feedbackService.GetTotalScore(x.InfluenterId.Value) }).OrderByDescending(o => o.score).Select(x => x.user).ToList();
                }
                else if (sortBy == 2)
                {
                    influencers = influencers.Select(x => new { user = x, score = _feedbackService.GetTotalScore(x.InfluenterId.Value) }).OrderBy(o => o.score).Select(x => x.user).ToList();
                }
                else if (sortBy == 3)
                {
                    influencers = influencers.Select(x => new { user = x, score = _feedbackService.GetInfluencerFeedbackCount(x.InfluenterId.Value) }).OrderByDescending(o => o.score).Select(x => x.user).ToList();
                }
                else if (sortBy == 4)
                {
                    influencers = influencers.Select(x => new { user = x, score = _feedbackService.GetInfluencerFeedbackCount(x.InfluenterId.Value) }).OrderBy(o => o.score).Select(x => x.user).ToList();
                }
                return influencers;
            }

            var influencerCat = _dbContext.InfluencerCategory.Where(x => kategorier.Contains(x.CategoryId)).ToList();
            var influencerPlat = _dbContext.InfluencerPlatform.Where(x => platforme.Contains(x.PlatformId)).ToList();

            var cResult = influencerCat.GroupBy(x => x.InfluencerId).Where(p => p.Count() >= kategorier.Count()).SelectMany(x => x).ToList()/*.Select(x => x.InfluencerId).Distinct()*/.ToList();
            var pResult = influencerPlat.GroupBy(x => x.InfluencerId).Where(p => p.Count() >= platforme.Count()).SelectMany(x => x).ToList()/*.Select(x => x.InfluencerId).Distinct()*/.ToList();

            var ids = new List<int>();

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


            var list = influencers.Where(x => ids.Contains(x.InfluenterId.Value)).ToList();

            if (sortBy == 1)
            {
                list = list.Select(x => new { user = x, score = _feedbackService.GetTotalScore(x.InfluenterId.Value) }).OrderByDescending(o => o.score).Select(x => x.user).ToList();
            }
            else if (sortBy == 2)
            {
                list = list.Select(x => new { user = x, score = _feedbackService.GetTotalScore(x.InfluenterId.Value) }).OrderBy(o => o.score).Select(x => x.user).ToList();
            }
            else if (sortBy == 3)
            {
                list = list.Select(x => new { user = x, score = _feedbackService.GetInfluencerFeedbackCount(x.InfluenterId.Value) }).OrderByDescending(o => o.score).Select(x => x.user).ToList();
            }
            else if (sortBy == 4)
            {
                list = list.Select(x => new { user = x, score = _feedbackService.GetInfluencerFeedbackCount(x.InfluenterId.Value) }).OrderBy(o => o.score).Select(x => x.user).ToList();
            }

            return list;
        }
    }
}
