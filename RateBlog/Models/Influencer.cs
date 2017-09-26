using Bestfluence.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models
{
    public class Influencer : BaseEntity
    {
        [Key, ForeignKey("ApplicationUser")]
        public override string Id { get; set; } 

        [Required(ErrorMessage = "Du skal udfylde Alias/kaldenavn")]
        public string Alias { get; set; }

        public string ProfileText { get; set; }

        [DefaultValue(false)]
        public bool IsApproved { get; set; }

        [MaxLength(30, ErrorMessage = "Din URL må være op til 30 karaktere langt.")]
        public string Url { get; set; }

        public virtual YoutubeData YoutubeData { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Feedback> Ratings { get; set; } 
        public virtual ICollection<InfluencerPlatform> InfluenterPlatform { get; set; }
        public virtual ICollection<InfluencerCategory> InfluenterKategori { get; set; }
    }
}
