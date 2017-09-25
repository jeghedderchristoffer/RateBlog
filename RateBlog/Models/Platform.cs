using Bestfluence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models
{
    public class Platform : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<InfluencerPlatform> InfluenterPlatform { get; set; }
    }
}
