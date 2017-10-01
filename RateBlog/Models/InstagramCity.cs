using Bestfluence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models
{
    public class InstagramCity : BaseEntity
    {
        public string InstagramDataId { get; set; }
        public virtual InstagramData InstagramData { get; set; }

        public string CityId { get; set; }
        public virtual City City { get; set; }

        public int Count { get; set; }
    }
}
