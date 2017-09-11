using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.AdminViewModels
{
    public class EditArticleViewModel
    {
        public string Id { get; set; } 

        public string Title { get; set; }

        public DateTime DateTime { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public string ArticleText { get; set; }

        public string Categories { get; set; }

        public IFormFile ArticlePicture { get; set; }

        public IFormFile IndexPicture { get; set; }
    }
}
