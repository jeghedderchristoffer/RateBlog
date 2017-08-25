using RateBlog.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class ReportFeedback : BaseEntity
    {
        
        public string FeedbackId { get; set; }
        public Feedback Feedback { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [DefaultValue(false)]
        public bool LanguageUse { get; set; }
        [DefaultValue(false)]
        public bool Spam { get; set; }
        [DefaultValue(false)]
        public bool Discrimination { get; set; }
        [DefaultValue(false)]
        public bool Other { get; set; }
        [DefaultValue(false)]
        public bool IsRead { get; set; }

        [DefaultValue(null)]
        [StringLength(200)]
        public string Description { get; set; }

        
    }
}
