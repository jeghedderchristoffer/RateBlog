using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bestfluence.Data;

namespace Bestfluence.Models
{
    public class RSSFeed : BaseEntity
    {

        public string Link { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PubData { get; set; }
        public int TimeSincePublished { get; set; }
        public string TimeString { get; set; } 
        public string Src { get; set; }
        public string Type { get; set; }
        public string Name { get; set; } 
    }
}
