using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models.InfluenterViewModels
{
    public class ShowViewModel
    {
        public Influencer Influenter { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ApplicationUser CurrentUser { get; set; } 
        public List<ApplicationUser> InfluentList { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public Vote Afstemning { get; set; }
        public bool? HasVoted { get; set; }
        public bool Follows { get; set; } 
    }
}
