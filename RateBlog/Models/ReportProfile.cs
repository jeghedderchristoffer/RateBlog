using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace RateBlog.Models
{
    public class ReportProfile
    {
        public int Id { get; set; }

        public int RatingId { get; set; }
        public Rating Rating { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public bool FakeProfile { get; set; }
        public bool Inappropriate { get; set; }
        public bool Other { get; set; }

        [StringLength(200)]
        public string Description { get; set; }


    }
}
