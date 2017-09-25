using Bestfluence.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models
{
    public class BlogRating : BaseEntity
    {
        public int Rate { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; } 

        [Required]
        public string BlogArticleId { get; set; } 
        public virtual BlogArticle BlogArticle { get; set; } 
    }
}
