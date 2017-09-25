using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models.AdminViewModels
{
    public class StatisticViewModel
    {
        public int UserCount { get; set; }
        public int RealUserCount { get; set; } 
        public int FeedbackCount { get; set; } 
        public int InfluencerCount { get; set; }
    }
}
