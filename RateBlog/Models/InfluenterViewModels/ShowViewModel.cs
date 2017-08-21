using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.InfluenterViewModels
{
    public class ShowViewModel
    {
        public Influencer Influenter { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<ApplicationUser> InfluentList { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }  
    }
}
