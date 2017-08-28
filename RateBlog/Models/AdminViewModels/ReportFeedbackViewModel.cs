using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.AdminViewModels
{
    public class ReportFeedbackViewModel
    {
        public List<ReportFeedback> ReportFeedbacks { get; set; }
        public ReportFeedback Report { get; set; }
        public ApplicationUser TheReportedUser { get; set; }

    }
}
