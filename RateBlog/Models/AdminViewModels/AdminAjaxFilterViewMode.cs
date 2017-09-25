using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models.AdminViewModels
{
    public class AdminAjaxFilterViewMode
    {
        public string Id { get; set; }
        public List<int> Platform { get; set; }
        public List<int> AgeGroup { get; set; }
        public string Gender { get; set; }
    }
}
