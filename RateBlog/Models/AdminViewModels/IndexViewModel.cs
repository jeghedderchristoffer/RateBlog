using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.AdminViewModels
{
    public class IndexViewModel
    {
        public List<ApplicationUser> AllUsers { get; set; }

        public List<ApplicationUser> InfluencerApprovedList { get; set; } 

    }
}
