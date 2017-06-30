using System;
using System.Collections.Generic;
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

        public int KommerUd { get; set; }
        public int Troværdighed { get; set; }
        public int Kvalitet { get; set; }
        public int Sprog { get; set; }
        public string Feedback { get; set; }

        public virtual ICollection<InfluenterRating> InfluenterRating { get; set; }
    }
}
