﻿using RateBlog.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class Influencer : BaseEntity
    {
        [Required(ErrorMessage = "Du skal udfylde Alias/kaldenavn")]
        public string Alias { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Feedback> Ratings { get; set; } 
        public virtual ICollection<InfluencerPlatform> InfluenterPlatform { get; set; }
        public virtual ICollection<InfluencerCategory> InfluenterKategori { get; set; }
    }
}