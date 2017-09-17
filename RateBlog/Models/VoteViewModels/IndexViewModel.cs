using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.VoteViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Vote> Votes { get; set; }
        public string InfluencerId { get; set; } 
    }
}
