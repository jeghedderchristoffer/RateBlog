using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models.AdminViewModels
{
    public class FeedbackViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }

        public Influencer Influencer { get; set; }

        public List<Feedback> Feedbacks { get; set; } 
    }
}
