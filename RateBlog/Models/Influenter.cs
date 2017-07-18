using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class Influenter
    {
        public int InfluenterId { get; set; }

        [Required(ErrorMessage = "Du skal udfylde dit Alias/kaldenavn")]
        public string Alias { get; set; }
        //public string Links { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; } 

        public virtual ICollection<InfluenterPlatform> InfluenterPlatform { get; set; }
        public virtual ICollection<InfluenterKategori> InfluenterKategori { get; set; }
    }
}
