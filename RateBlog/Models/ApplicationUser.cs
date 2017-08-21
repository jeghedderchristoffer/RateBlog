﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RateBlog.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
       
        public DateTime BirthDay { get; set; } 
       
        public int Postnummer { get; set; } 

        public string ProfileText { get; set; }

        public string Gender { get; set; } 

        public Byte[] ProfilePicture { get; set; }

        public virtual ICollection<Feedback> Ratings { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastLogin { get; set; }

        public DateTime TermsAndConditions { get; set; }

        public bool NewsLetter { get; set; } 


    }
}
