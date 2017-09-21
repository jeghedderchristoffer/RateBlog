using RateBlog.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class BlogArticle : BaseEntity
    {
        public string Title { get; set; }

        public DateTime DateTime { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public string ArticleText { get; set; } 

        public string Categories { get; set; }

        public Byte[] ArticlePicture { get; set; }

        public Byte[] IndexPicture { get; set; }

        [DefaultValue(false)]
        public bool Publish { get; set; }

        public ICollection<BlogRating> BlogRatings { get; set; }

        public ICollection<BlogComment> BlogComments { get; set; } 
    }
}
