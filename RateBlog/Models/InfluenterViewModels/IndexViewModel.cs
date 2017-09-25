using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models.InfluenterViewModels
{
    public class IndexViewModel
    {
        public string SearchString { get; set; }
        public List<Influencer> InfluencerList  { get; set; }       
        public Dictionary<string, string> CategoryIds { get; set; }
        public Dictionary<string, string> PlatformIds { get; set; } 
    }
}
