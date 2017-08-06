using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.AdminViewModels
{
    public class SeFeedbackViewModel
    {
        public List<Rating> ListRating { get; set; }
        public Rating Rating { get; set; }
        public string AnmelderNavn { get; set; }

    }
}
