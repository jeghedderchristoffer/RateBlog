using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class Rating
    {
        // tror den mangler en FK til applicationUser, da man skal kunne fortælle
        // hvem der har givet denne rating. Og det skulle gerne være 1-1 forhold. 

        public int RatingId { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int InfluenterId { get; set; }
        public Influenter Influenter { get; set; }

        public DateTime RateDateTime { get; set; } 

        public int Kvalitet { get; set; }
        public int Troværdighed { get; set; }
        public int Interaktion { get; set; }
        public int Opførsel { get; set; }
        public bool Anbefaling { get; set; }

        public string Feedback { get; set; }

        [DefaultValue(null)]
        public string Answer { get; set; }

        [DefaultValue(false)]
        public bool IsRead { get; set; }
    }
}
