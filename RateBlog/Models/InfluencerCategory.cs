using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class InfluencerCategory
    {
        public string InfluencerId { get; set; }
        public Influencer Influencer { get; set; }
        public string CategoryId { get; set; }
        public Category Category { get; set; } 
    }
}
