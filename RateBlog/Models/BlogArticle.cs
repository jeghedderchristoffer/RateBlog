using RateBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class BlogArticle : BaseEntity
    {
        public string DateTime { get; set; }
        public string ArticleHeader { get; set; }
        public string ArticleText { get; set; }
        public string BriefText { get; set; }
        public string Author { get; set; }
        public string Categories { get; set; }
        public Byte[] ArticlePicture { get; set; }
        public Byte[] IndexPicture { get; set; }
    }
}
