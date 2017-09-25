using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models
{
    public class VoteAnswer
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string VoteQuestionId { get; set; }
        public virtual VoteQuestion VoteQuestion { get; set; } 
    }
}
