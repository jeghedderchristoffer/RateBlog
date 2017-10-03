using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Bestfluence.Data;

namespace Bestfluence.Models
{
    public class YoutubeData : BaseEntity
    {
        public string InfluencerId { get; set; }
        public virtual Influencer Influencer { get; set; }

        public virtual YoutubeAgeGroup YoutubeAgeGroup { get; set; }
        public virtual ICollection<YoutubeCountry> YoutubeCountry { get; set; }

        public int Engagement { get; set; }
        public int Views { get; set; }
        public double MaleViews { get; set; }
        public double FemaleViews { get; set; }
        public int Subcribers { get; set; }

        public int Likes { get; set; }
        public int Dislike { get; set; }
        public int Comments { get; set; }
        

    }
}
