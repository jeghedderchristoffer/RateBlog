using RateBlog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RateBlog.Models;
using Microsoft.AspNetCore.Identity;
using RateBlog.Data;
using RateBlog.Repository;

namespace RateBlog.Services
{
    public class PlatformCategoryService : IPlatformCategoryService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IRepository<Category> _categoryRepo;
        private readonly IRepository<Platform> _platformRepo;
        private readonly IRepository<Influencer> _influencerRepo;


        public PlatformCategoryService(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, IRepository<Category> categoryRepo, IRepository<Platform> platformRepo, IRepository<Influencer> influencerRepo)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _categoryRepo = categoryRepo;
            _platformRepo = platformRepo;
            _influencerRepo = influencerRepo; 
        }

        public IEnumerable<InfluencerPlatform> GetAllInfluencerPlatformForInfluencer(string id)
        {
            if (_dbContext.InfluencerPlatform.Any(x => x.InfluencerId == id))
            {
                return _dbContext.InfluencerPlatform.Where(x => x.InfluencerId == id);
            }

            return null;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllInfluencersWithCategory(string name)
        {
            var id = GetCategoryIdByName(name);
            var ik = _dbContext.InfluencerCategory.Where(x => x.CategoryId == id);

            List<ApplicationUser> userList = new List<ApplicationUser>();

            foreach (var v in ik)
            {
                var influencer = _influencerRepo.Get(v.InfluencerId);
                var user = await _userManager.FindByIdAsync(influencer.Id); 

                if (!user.Name.ToLower().Contains(name) && !influencer.Alias.ToLower().Contains(name))
                {
                    userList.Add(user);
                }
            }

            return userList;
        }
       
        public async Task<IEnumerable<ApplicationUser>> GetAllInfluencersWithPlatform(string name)
        {
            var id = GetPlatformIdByName(name);
            var ik = _dbContext.InfluencerPlatform.Where(x => x.PlatformId == id);

            List<ApplicationUser> userList = new List<ApplicationUser>();

            foreach (var v in ik)
            {
                var influencer = _influencerRepo.Get(v.InfluencerId);
                var user = await _userManager.FindByIdAsync(influencer.Id);

                if (!user.Name.ToLower().Contains(name) && !influencer.Alias.ToLower().Contains(name))
                {
                    userList.Add(user);
                }
            }
            return userList;
        }

        public string GetCategoryIdByName(string name)
        {
            if (_categoryRepo.GetAll().Any(x => x.Name.ToLower() == name.ToLower()))
            {
                return _categoryRepo.GetAll().SingleOrDefault(x => x.Name.ToLower() == name.ToLower()).Id;
            }
            return null;
        }

        public IEnumerable<string> GetInfluencerCategoryNames(string id)
        {
            if (_dbContext.InfluencerCategory.Any(x => x.InfluencerId == id))
            {
                var list = new List<string>();
                foreach (var v in _dbContext.InfluencerCategory.Where(x => x.InfluencerId == id))
                {
                    list.Add(_categoryRepo.Get(v.CategoryId).Name);
                }
                return list;
            }
            return null;
        }

        public string GetPlatformIdByName(string name)
        {
            if (_dbContext.Platform.Any(x => x.Name.ToLower() == name))
            {
                return _dbContext.Platform.SingleOrDefault(x => x.Name == name).Id;
            }
            return null;
        }

        public string GetPlatformLink(string influencerId, string platformId)
        {
            if (_dbContext.InfluencerPlatform.Any(x => x.InfluencerId == influencerId && x.PlatformId == platformId))
            {
                return _dbContext.InfluencerPlatform.SingleOrDefault(x => x.InfluencerId == influencerId && x.PlatformId == platformId).Link;
            }
            return null;
        }

        public void InsertCategory(string influencerId, string categoryId, bool isSelected)
        {
            InfluencerCategory ik = new InfluencerCategory()
            {
                InfluencerId = influencerId,
                CategoryId = categoryId
            };

            if (_dbContext.InfluencerCategory.Any(x => x.InfluencerId == influencerId && x.CategoryId == categoryId))
            {
                if (!isSelected)
                {
                    _dbContext.Remove(ik);
                    _dbContext.SaveChanges();
                }
            }
            else
            {
                if (isSelected)
                {
                    _dbContext.Add(ik);
                    _dbContext.SaveChanges();
                }
            }
        }

        public void InsertPlatform(string influencerId, string platformId, string link)
        {
            InfluencerPlatform ip = new InfluencerPlatform()
            {
                InfluencerId = influencerId,
                PlatformId = platformId,
                Link = link
            };

            // Skal slettes hvis den eksistere OG string.IsNullOrEmpty
            if (_dbContext.InfluencerPlatform.Any(x => x.InfluencerId == influencerId && x.PlatformId == platformId))
            {
                if (string.IsNullOrEmpty(link))
                {
                    _dbContext.InfluencerPlatform.Remove(ip);
                }
                else
                {
                    _dbContext.InfluencerPlatform.Update(ip);
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(link))
                {
                    _dbContext.InfluencerPlatform.Add(ip);
                }
            }

            _dbContext.SaveChanges();
        }

        public bool IsCategorySelected(string influencerId, string categoryId)
        {
            if (_dbContext.InfluencerCategory.Any(x => x.InfluencerId == influencerId && x.CategoryId == categoryId))
            {
                return true;
            }
            return false;
        }
    }
}
