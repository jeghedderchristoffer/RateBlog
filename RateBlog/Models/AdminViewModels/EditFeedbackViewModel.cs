using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.AdminViewModels
{
    public class EditFeedbackViewModel
    {
        public Feedback Feedback { get; set; }

        public bool IsInfluencer { get; set; } 
    }
}
