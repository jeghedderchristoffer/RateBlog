using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models.InfluenterViewModels
{
    public class ReadViewModel
    {

        public Influencer Influenter { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ApplicationUser CurrentApplicationUser { get; set; } 
        public int Age { get; set; }  
        public string Gender { get; set; }
        public bool Follows { get; set; } 
    }
}
