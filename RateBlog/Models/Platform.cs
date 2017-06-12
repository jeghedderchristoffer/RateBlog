using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class Platform
    {

        public int PlatformId { get; set; }
        public string PlatformNavn { get; set; }

       public virtual ICollection<InfluenterPlatform> InfluenterPlatform { get; set; }
    }
}
