using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bestfluence.Data;

namespace Bestfluence.Models
{
    public class YoutubeCountry : BaseEntity
    {
        public string YoutubeDataId { get; set; }
        public virtual YoutubeData YoutubeData { get; set; }

        public string CountryId { get; set; }
        public virtual Country Country { get; set; }

        public int Count { get; set; }
    }
}
