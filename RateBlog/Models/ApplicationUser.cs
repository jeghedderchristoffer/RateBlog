using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Bestfluence.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}")]
        public DateTime BirthDay { get; set; } 
       
        public int Postnummer { get; set; } 

        public string Gender { get; set; } 

        public Byte[] ProfilePicture { get; set; }

        public DateTime Created { get; set; }

        public DateTime TermsAndConditions { get; set; }

        public bool NewsLetter { get; set; }

        public virtual EmailNotification EmailNotification { get; set; }
        public virtual ICollection<BlogComment> BlogComments { get; set; } 
        public virtual ICollection<BlogRating> BlogRatings { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<FeedbackReport> FeedbackReports { get; set; }
        public virtual ICollection<VoteAnswer> VoteAnswers { get; set; }
        public virtual Influencer Influencer { get; set; } 

    }
}
