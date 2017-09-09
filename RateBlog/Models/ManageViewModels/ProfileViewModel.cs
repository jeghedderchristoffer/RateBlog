using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RateBlog.Models.ManageViewModels
{
    public class ProfileViewModel
    {
        public ApplicationUser ApplicationUser { get; set; } 

        public int Age { get; set; } 

        public string Gender { get; set; }
    }
}
