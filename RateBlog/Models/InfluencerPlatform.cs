using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class InfluencerPlatform
    {

        public string InfluencerId { get; set; }
        public Influencer Influencer { get; set; }

        public string PlatformId { get; set; }
        public Platform Platform { get; set; }

        public string Link { get; set; }
    }
}
