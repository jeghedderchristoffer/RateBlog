using Bestfluence.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models.AdminViewModels
{
    public class IndexViewModel
    {
        public List<ApplicationUser> AllUsers { get; set; }

        public IEnumerable<Influencer> NotApprovedList { get; set; }

        public IEnumerable<DisplayFeedbackReports> FeedbackReports { get; set; } 

    }
}
