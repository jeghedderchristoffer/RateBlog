using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.AccountViewModels
{
    public class ProfileViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public string AgeGroup { get; set; }
        public string Gender { get; set; } 
    }
}
