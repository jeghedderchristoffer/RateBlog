using RateBlog.Data;
using RateBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Repository
{
    public class InfluenterPlatformRepository : IInfluenterPlatformRepository
    {
        private ApplicationDbContext _dbContext;

        public InfluenterPlatformRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<InfluenterPlatform> GetAllByInfluenter(int inluenterId)
        {
            if(_dbContext.InfluenterPlatform.Any(x=> x.InfluenterId == inluenterId))
            {
                return _dbContext.InfluenterPlatform.Where(x => x.InfluenterId == inluenterId).ToList();            
            }

            return null; 
        }

        public string GetLink(int influenterId, int platformId)
        {
            if (_dbContext.InfluenterPlatform.Any(x => x.InfluenterId == influenterId && x.PlatformId == platformId))
            {
                return _dbContext.InfluenterPlatform.SingleOrDefault(x => x.InfluenterId == influenterId && x.PlatformId == platformId).Link;
            }
            return null;
        }

        public void Insert(int influenterId, int platformId, string link)
        {
            InfluenterPlatform ip = new InfluenterPlatform()
            {
                InfluenterId = influenterId,
                PlatformId = platformId,
                Link = link
            };

            // Skal slettes hvis den eksistere OG string.IsNullOrEmpty
            if (_dbContext.InfluenterPlatform.Any(x => x.InfluenterId == influenterId && x.PlatformId == platformId))
            {
                if (string.IsNullOrEmpty(link))
                {
                    _dbContext.InfluenterPlatform.Remove(ip);
                }
                else
                {
                    _dbContext.InfluenterPlatform.Update(ip);
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(link))
                {
                    _dbContext.InfluenterPlatform.Add(ip);
                }
            }

            _dbContext.SaveChanges();
        }
    }
}
