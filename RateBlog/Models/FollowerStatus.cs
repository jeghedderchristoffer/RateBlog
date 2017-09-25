using Bestfluence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models
{
    public class FollowerStatus : BaseEntity
    {
        public int Score { get; set; } 

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public string FollowerRankId { get; set; } 
        public virtual FollowerRank FollowerRank { get; set; } 
    }
}
