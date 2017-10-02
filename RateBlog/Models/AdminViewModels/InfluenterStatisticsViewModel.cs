using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models.AdminViewModels
{
    public class InfluenterStatisticsViewModel
    {
        public ApplicationUser InfluenterUserInfo { get; set; }
        public List<Feedback> InfluentersFeedbacks { get; set; }
        public Influencer Influenter { get; set; }
        public bool HasInstagramData { get; set; }
    }
}
