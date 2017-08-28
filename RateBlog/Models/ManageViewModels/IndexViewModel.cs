using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RateBlog.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }

        public IList<UserLoginInfo> Logins { get; set; }

        public string PhoneNumber { get; set; }

        public bool TwoFactor { get; set; }

        public bool BrowserRemembered { get; set; }

        public ApplicationUser ApplicationUser { get; set; } 

        public int Age { get; set; } 

        public string Gender { get; set; }
    }
}
