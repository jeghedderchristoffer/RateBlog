using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.InfluenterViewModels
{
    public class ShowViewModel
    {
        public Influenter Influenter { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<ApplicationUser> InfluentList { get; set; }
    }
}
