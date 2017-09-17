using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class VoteQuestion
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Question { get; set; }

        [Required]
        public string VoteId { get; set; }
        public virtual Vote Vote { get; set; }

        public virtual ICollection<VoteAnswer> VoteAnswers { get; set; } 
    }
}
