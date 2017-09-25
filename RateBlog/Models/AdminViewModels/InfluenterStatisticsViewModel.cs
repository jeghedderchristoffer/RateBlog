using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models.AdminViewModels
{
    public class InfluenterStatisticsViewModel
    {
        public ApplicationUser Influenter { get; set; }
        public List<Feedback> InfluentersFeedbacks { get; set; }

    }
}
