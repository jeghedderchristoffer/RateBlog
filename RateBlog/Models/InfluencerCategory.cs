using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class InfluencerCategory
    {
        public int InfluencerId { get; set; }
        public Influencer Influencer { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } 
    }
}
