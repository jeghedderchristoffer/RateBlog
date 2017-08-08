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

        public PlatformCategoryService(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, IRepository<Category> categoryRepo)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _categoryRepo = categoryRepo; 
        }

        public IEnumerable<InfluencerPlatform> GetAllInfluencerPlatformForInfluencer(int id)
        {
            if (_dbContext.InfluencerPlatform.Any(x => x.InfluencerId == id))
            {
                return _dbContext.InfluencerPlatform.Where(x => x.InfluencerId == id);
            }

            return null;
        }

        public IEnumerable<ApplicationUser> GetAllInfluencersWithCategory(string name)
        {
            var id = GetCategoryIdByName(name);
            var ik = _dbContext.InfluencerCategory.Where(x => x.CategoryId == id);

            List<ApplicationUser> userList = new List<ApplicationUser>();

            foreach (var v in ik)
            {
                userList.Add(_userManager.Users.SingleOrDefault(x => x.InfluenterId == v.InfluencerId));
            }

            return userList;
        }

        public IEnumerable<ApplicationUser> GetAllInfluencersWithPlatform(string name)
        {
            var id = GetPlatformIdByName(name);
            var ik = _dbContext.InfluencerPlatform.Where(x => x.PlatformId == id);

            List<ApplicationUser> userList = new List<ApplicationUser>();

            foreach (var v in ik)
            {
                userList.Add(_userManager.Users.SingleOrDefault(x => x.InfluenterId == v.InfluencerId));
            }
            return userList;
        }

        public int GetCategoryIdByName(string name)
        {
            if (_categoryRepo.GetAll().Any(x => x.Name == name))
            {
                return _categoryRepo.GetAll().SingleOrDefault(x => x.Name == name).Id;
            }
            return 0;
        }

        public IEnumerable<string> GetInfluencerCategoryNames(int id)
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

        public int GetPlatformIdByName(string name)
        {
            if (_dbContext.Platform.Any(x => x.Name == name))
            {
                return _dbContext.Platform.SingleOrDefault(x => x.Name == name).Id;
            }
            return 0;
        }

        public string GetPlatformLink(int influencerId, int platformId)
        {
            if (_dbContext.InfluencerPlatform.Any(x => x.InfluencerId == influencerId && x.PlatformId == platformId))
            {
                return _dbContext.InfluencerPlatform.SingleOrDefault(x => x.InfluencerId == influencerId && x.PlatformId == platformId).Link;
            }
            return null;
        }

        public void InsertCategory(int influencerId, int categoryId, bool isSelected)
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

        public void InsertPlatform(int influencerId, int platformId, string link)
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

        public bool IsCategorySelected(int influencerId, int categoryId)
        {
            if (_dbContext.InfluencerCategory.Any(x => x.InfluencerId == influencerId && x.CategoryId == categoryId))
            {
                return true;
            }
            return false;
        }

        public IEnumerable<ApplicationUser> SortInfluencerByPlatAndCat(int[] platformIds, int[] categoryIds, List<ApplicationUser> users)
        {
            var influencerIds = new List<int>();
            var resultUserList = new List<ApplicationUser>();

            var resultCat = new List<InfluencerCategory>();
            var resultPlat = new List<InfluencerPlatform>(); 

            foreach (var v in users)
            {
                influencerIds.Add(v.InfluenterId.Value);
            }

            if (platformIds.Count() == 0 && categoryIds.Count() == 0)
            {
                return users;
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
    }
}
