using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RateBlog.Models.BlogViewModel
{
    public class BlogViewModel
    {
        public IFormFile ArticlesPicture { get; set; }
        public IFormFile IndexPicture { get; set; }
        public string SearchString { get; set; }
        public List<Blog> BlogList { get; set; }
        public Blog Blog { get; set; }
        

    }
}
