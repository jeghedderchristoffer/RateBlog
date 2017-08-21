using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.AdminViewModels
{
    public class EditUserViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public Influencer Influencer { get; set; } 
    }
}
