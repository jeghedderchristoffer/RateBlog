using Bestfluence.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models
{
    public class FeedbackReport : BaseEntity
    {
        public string FeedbackId { get; set; }
        public Feedback Feedback { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } 

        public string Reason { get; set; } 

        public DateTime DateTime { get; set; }

        [DefaultValue(false)]
        public bool IsRead { get; set; } 

        [DefaultValue(null)]
        [StringLength(200)]
        public string Description { get; set; }
    }
}
