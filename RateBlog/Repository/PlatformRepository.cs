using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RateBlog.Models;
using RateBlog.Data;

namespace RateBlog.Repository
{
    public class PlatformRepository : IPlatformRepository
    {
        private ApplicationDbContext _applicationDbContext;

        public PlatformRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            
        }
        public Platform Get(int Id)
        {
            return _applicationDbContext.Platform.FirstOrDefault(x => x.PlatformId == Id);
        }

        public List<Platform> GetAll()
        {
            return _applicationDbContext.Platform.ToList();
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
            if (_applicationDbContext.InfluenterPlatform.Any(x => x.InfluenterId == influenterId && x.PlatformId == platformId))
            {
                if (string.IsNullOrEmpty(link))
                {
                    _applicationDbContext.InfluenterPlatform.Remove(ip);
                }
                else
                {
                    _applicationDbContext.InfluenterPlatform.Update(ip);
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(link))
                {
                    _applicationDbContext.InfluenterPlatform.Add(ip);
                }
            }

            _applicationDbContext.SaveChanges();
        }

        public string GetLink(int influenterId, int platformId)
        {
            if (_applicationDbContext.InfluenterPlatform.Any(x => x.InfluenterId == influenterId && x.PlatformId == platformId))
            {
                return _applicationDbContext.InfluenterPlatform.SingleOrDefault(x => x.InfluenterId == influenterId && x.PlatformId == platformId).Link;
            }
            return null;
        }

        public List<InfluenterPlatform> GetAllInfluenterPlatformForInfluenter(int inluenterId)
        {
            if (_applicationDbContext.InfluenterPlatform.Any(x => x.InfluenterId == inluenterId))
            {
                return _applicationDbContext.InfluenterPlatform.Where(x => x.InfluenterId == inluenterId).ToList();
            }

            return null;
        }
    }
}
