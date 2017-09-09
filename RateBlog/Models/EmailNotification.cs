using RateBlog.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class EmailNotification : BaseEntity
    {
        [Key, ForeignKey("ApplicationUser")]
        public override string Id { get; set; }

        [DefaultValue(true)]
        public bool FeedbackUpdate { get; set; }

        [DefaultValue(false)]
        public bool NewsLetter { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; } 
    }
}
