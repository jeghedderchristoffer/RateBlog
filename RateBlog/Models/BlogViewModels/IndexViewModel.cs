﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.BlogViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<BlogArticle> ArticleList { get; set; } 
    }
}