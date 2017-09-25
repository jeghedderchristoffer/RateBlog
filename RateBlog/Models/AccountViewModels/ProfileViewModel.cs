using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models.AccountViewModels
{
    public class ProfileViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; } 
    }
}
