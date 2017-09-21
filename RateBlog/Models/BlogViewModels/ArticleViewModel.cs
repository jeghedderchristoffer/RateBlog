using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.BlogViewModels
{
    public class ArticleViewModel
    {
        public BlogArticle Article { get; set; }
        public bool HasVoted { get; set; }
        public string ElementToScroll { get; set; }
    }
}
