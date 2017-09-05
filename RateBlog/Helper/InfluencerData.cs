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
        public IEnumerable<double> FeedbackScore { get; set; }
        public IEnumerable<string> Categories { get; set; } 
        public IEnumerable<string> Platforms { get; set; }
        public string ProfileText { get; set; } 
    }
}
