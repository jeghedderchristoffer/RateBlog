using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Helper
{
    public class InfluencerData
    {
        public string Id { get; set; }
        public string Alias { get; set; }
        public int FeedbackCount { get; set; }
        public double FeedbackScore { get; set; }
        public string Categories { get; set; } 
        public string Platforms { get; set; }
    }
}
