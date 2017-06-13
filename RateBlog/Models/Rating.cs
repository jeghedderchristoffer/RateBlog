using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class Rating
    {

        public int RatingId { get; set; }
        public int KommerUd { get; set; }
        public int Troværdighed { get; set; }
        public int Kvalitet { get; set; }
        public int Sprog { get; set; }
        public string Feedback { get; set; }




        public virtual ICollection<InfluenterRating> InfluenterRating { get; set; }
    }
}
