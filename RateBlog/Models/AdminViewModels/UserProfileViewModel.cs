using RateBlog.Models.ManageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.AdminViewModels
{
    public class UserProfileViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public EditViewModel EditProfileViewModel { get; set; }
        public InfluencerViewModel InfluencerViewModel { get; set; } 
    }
}
