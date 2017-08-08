using RateBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } 
        public virtual ICollection<InfluencerCategory> InfluenterKategori { get; set; }
    }
}
