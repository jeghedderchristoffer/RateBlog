using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Bestfluence.Models.AdminViewModels
{
    public class CreateArticlesViewModel
    {
        public string Title { get; set; }

        public DateTime DateTime { get; set; }

        public string Url { get; set; } 

        public string Author { get; set; }

        public string Description { get; set; }

        public string ArticleText { get; set; }

        public string Categories { get; set; }

        public IFormFile ArticlePicture { get; set; }

        public IFormFile IndexPicture { get; set; }
    }
}
