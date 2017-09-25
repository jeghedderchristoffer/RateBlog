using Bestfluence.Services.Interfaces;
using Bestfluence.Data;
using Bestfluence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Services
{
    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext _dbContext;

        public BlogService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext; 
        }

        public IEnumerable<BlogArticle> GetLast4BlogArticle()
        {
            return _dbContext.BlogArticles.OrderByDescending(x => x.DateTime).Where(x => x.Publish == true).Take(4); 
        }
    }
}
