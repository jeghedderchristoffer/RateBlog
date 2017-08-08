using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.RatingViewModels
{
    public class RatingViewModel
    {
        // Hvem er influenteren??
        public Influencer Influenter { get; set; }

        // Hvad står der i Rating-reviewet? Teksten altså...
        public string Review { get; set; }
    }
}
