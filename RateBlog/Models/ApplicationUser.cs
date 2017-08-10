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
       
        public int? Year { get; set; }
       
        public int? Postnummer { get; set; } 

        public string ProfileText { get; set; }

        public Byte[] ProfilePicture { get; set; } 

        public virtual Influencer Influenter { get; set; }
        public int? InfluenterId { get; set; }



    }
}
