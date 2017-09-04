using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RateBlog.Models.BlogViewModel
{
    public class CreateArticlesViewModel
    {

        public string DateTime { get; set; }
        public string ArticleHeader { get; set; }
        public string ArticleText { get; set; }
        public string BriefText { get; set; }
        public string Author { get; set; }
        public string Categories { get; set; }
        public IFormFile ArticlesPicture { get; set; }
        public IFormFile IndexPicture { get; set; }

    }
}
