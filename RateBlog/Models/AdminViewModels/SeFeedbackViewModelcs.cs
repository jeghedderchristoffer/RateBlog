using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.AdminViewModels
{
    public class SeFeedbackViewModel
    {
        public List<Feedback> ListRating { get; set; }
        public Feedback feedBack { get; set; }
        public string AnmelderNavn { get; set; }

    }
}
