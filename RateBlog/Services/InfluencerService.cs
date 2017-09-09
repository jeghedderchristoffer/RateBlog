using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RateBlog.Data;
using RateBlog.Models;
using RateBlog.Repository;
using RateBlog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Services
{
    public class InfluencerService : IInfluencerService
    {
        private readonly ApplicationDbContext _dbContext;

        public InfluencerService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Influencer> GetAll(string search)
        {
            foreach(var v in GetPlatformNames())
            {
                if (search.ToLower().Equals(v.ToLower()))
                {
                    return _dbContext.Influencer.Include(x => x.InfluenterPlatform).ThenInclude(x => x.Platform).Where(x => x.InfluenterPlatform.Any(p => p.Platform.Name == v)) 
                        .Include(x => x.InfluenterKategori).ThenInclude(x => x.Category)
                        .Include(x => x.Ratings);
                }
            }
            
            foreach(var v in GetCategoryNames())
            {
                if (search.ToLower().Equals(v.ToLower()))
                {
                    return _dbContext.Influencer.Include(x => x.InfluenterKategori).ThenInclude(x => x.Category).Where(x => x.InfluenterKategori.Any(p => p.Category.Name == v))
                        .Include(x => x.InfluenterPlatform).ThenInclude(x => x.Platform)
                        .Include(x => x.Ratings);
                }
            }

            return _dbContext.Influencer.Where(x => x.Alias.ToLower().Contains(search.ToLower()) && x.IsApproved == true)
                .Include(x => x.InfluenterKategori).ThenInclude(x => x.Category)
                .Include(x => x.InfluenterPlatform).ThenInclude(x => x.Platform)
                .Include(x => x.Ratings);
        }

        public async Task<Influencer> GetInfluecerAsync(string id)
        {
            return await _dbContext.Influencer
                .Include(x => x.InfluenterKategori).ThenInclude(x => x.Category)
                .Include(x => x.InfluenterPlatform).ThenInclude(x => x.Platform)
                .Include(x => x.Ratings)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public IEnumerable<Influencer> GetUnApprovedInfluencers()
        {
            return _dbContext.Influencer.Where(x => x.IsApproved == false); 
        }

        public bool IsInfluencer(string id)
        {
            return _dbContext.Influencer.Any(x => x.Id == id); 
        }

        private IEnumerable<string> GetPlatformNames()
        {
            return new string[]
            {
                "YouTube",
                "Facebook",
                "SnapChat",
                "Twitch",
                "Website",
                "Twitter",
                "Instagram"
            };
        }
        private IEnumerable<string> GetCategoryNames()
        {
            return new string[]
            {
                "Gaming",
                "Personal",
                "Interests",
                "Entertainment",
                "Fashion",
                "Lifestyle",
                "Beauty"
            };
        }
    }
}
