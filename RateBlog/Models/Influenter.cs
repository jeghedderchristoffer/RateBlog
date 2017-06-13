using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class Influenter
    {
       
        public int InfluenterId { get; set; }
        public string Fornavn { get; set; }
        public string Efternavn { get; set; }
        public string Alias { get; set; }
        public int? Alder { get; set; }
        public string Links { get; set; }
        public string Profiltekst { get; set; }

        public virtual ICollection<InfluenterPlatform> InfluenterPlatform { get; set; }
        public virtual ICollection<InfluenterRating> InfluenterRating { get; set; }


    }
}
