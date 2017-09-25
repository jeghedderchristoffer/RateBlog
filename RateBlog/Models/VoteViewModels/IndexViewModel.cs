using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models.VoteViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Vote> Votes { get; set; }
        public Influencer Influencer { get; set; }  
    }
}
