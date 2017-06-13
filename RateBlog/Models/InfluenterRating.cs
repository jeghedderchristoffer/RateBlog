using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class InfluenterRating
    {

        public int InfluenterId { get; set; }
        public Influenter Influenter { get; set; }


        public int RatingId { get; set; }
        public Rating Rating { get; set; }


    }
}
