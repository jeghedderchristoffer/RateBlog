using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.AdminViewModels
{
    public class BlogArticlesViewModel
    {
        public IEnumerable<BlogArticle> Articles { get; set; } 
    }
}
