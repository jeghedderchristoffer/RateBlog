using RateBlog.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class Feedback : BaseEntity
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int InfluenterId { get; set; }
        public Influencer Influenter { get; set; }

        public DateTime RateDateTime { get; set; } 

        public int Kvalitet { get; set; }
        public int Troværdighed { get; set; }
        public int Interaktion { get; set; }
        public int Opførsel { get; set; }

        public bool? Anbefaling { get; set; }

        public string FeedbackText { get; set; } 

        [DefaultValue(null)]
        public string Answer { get; set; }

        [DefaultValue(false)]
        public bool IsRead { get; set; }

        [DefaultValue(false)]
        public bool IsAnswerRead { get; set; }

    }
}
