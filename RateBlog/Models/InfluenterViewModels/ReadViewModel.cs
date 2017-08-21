using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.InfluenterViewModels
{
    public class ReadViewModel
    {

        public Influencer Influenter { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int Age { get; set; }  
        public string Gender { get; set; }

    }
}
