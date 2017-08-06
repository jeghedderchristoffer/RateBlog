using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.AdminViewModels
{
    public class IndexViewModel
    {
        
       
        public string SearchString { get; set; }
        public List<ApplicationUser> InfluentList { get; set; }
        public bool isInfluencer { get; set; }
  
    }
}
