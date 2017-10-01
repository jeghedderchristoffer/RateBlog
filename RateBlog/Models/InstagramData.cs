using Bestfluence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models
{
    public class InstagramData: BaseEntity
    {
        public string InfluencerId { get; set; }
        public virtual Influencer Influencer { get; set; }

        public virtual InstagramAgeGroup InstagramAgeGroup { get; set; }
        public virtual ICollection<InstagramCountry> InstagramCountry { get; set; }
        public virtual ICollection<InstagramCity> InstagramCity { get; set; }

        public int FollowerCount { get; set; }
        public int MediaCount { get; set; }

        public int DayReach { get; set; }
        public int WeekReach { get; set; }
        public int MonthReach { get; set; }
        public int DayImpression { get; set; }
        public int WeekImpression { get; set; }
        public int MonthImpression { get; set; }



    }
}
