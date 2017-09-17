using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class Vote
    {
        [Key]
        public string Id { get; set; }  

        [Required]
        public string Title { get; set; } 

        [DefaultValue(true)]
        public bool Active { get; set; }

        public DateTime DateTime { get; set; } 

        [Required]
        public string InfluencerId { get; set; }
        public virtual Influencer Influencer { get; set; }

        public virtual ICollection<VoteQuestion> VoteQuestions { get; set; } 
    }
}
