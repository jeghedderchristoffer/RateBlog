using RateBlog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RateBlog.Models;
using RateBlog.Data;
using Microsoft.AspNetCore.Identity;

namespace RateBlog.Services
{
    public class SortService : ISortService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFeedbackService _feedbackService;

        public SortService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IFeedbackService feedbackService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _feedbackService = feedbackService;
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
    }
}
