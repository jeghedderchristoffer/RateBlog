using RateBlog.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class BlogComment : BaseEntity
    {
        public string Comment { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public DateTime DateTime { get; set; } 

        [Required]
        public string BlogArticleId { get; set; }
        public virtual BlogArticle BlogArticle { get; set; } 
    }
}
