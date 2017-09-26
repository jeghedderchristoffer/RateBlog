using Bestfluence.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class InstagramApiData : BaseEntity
    {
        [Key, ForeignKey("ApplicationUser")]
        public override string Id { get; set; }

        public string InstagramJson{ get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
