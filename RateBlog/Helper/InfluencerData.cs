using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Helper
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
        public bool InfluencerVote { get; set; }
        public string Url { get; set; }
        public string ValidatedInfluencer { get; set; }
        public bool Follows { get; set; }
        public int FollowerCount { get; set; } 
    }
}
