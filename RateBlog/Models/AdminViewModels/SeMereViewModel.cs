using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.AdminViewModels
{
    public class SeMereViewModel
    {
        public Influenter Influenter { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<ApplicationUser> InfluentList { get; set; }
    }
}
