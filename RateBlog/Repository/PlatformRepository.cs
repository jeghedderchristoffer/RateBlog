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
    }
}
