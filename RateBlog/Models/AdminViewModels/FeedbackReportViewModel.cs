using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models.AdminViewModels
{
    public class FeedbackReportViewModel
    {
        public Feedback Feedback { get; set; }
        public IEnumerable<FeedbackReport> FeedbackReports { get; set; }  
    }
}
