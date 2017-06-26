using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class InfluenterPlatform
    {

        public int InfluenterId { get; set; }
        public Influenter Influenter { get; set; }

        public int PlatformId { get; set; }
        public Platform Platform { get; set; }

        public string Link { get; set; }
    }
}
