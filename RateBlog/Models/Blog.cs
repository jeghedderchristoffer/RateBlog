using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RateBlog.Data;

namespace RateBlog.Models
{
    public class Blog : BaseEntity
    {
      
        public int DateTime { get; set; }
        public string ArticleHeader { get; set; }
        public string ArticleText { get; set; }
        public string BriefText { get; set; }
        public string PictureLink { get; set; }


    }
}
