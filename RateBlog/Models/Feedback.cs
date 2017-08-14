using RateBlog.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class Feedback : BaseEntity
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string InfluenterId { get; set; }
        public Influencer Influenter { get; set; }

        public DateTime FeedbackDateTime { get; set; } 

        public int Kvalitet { get; set; }
        public int Troværdighed { get; set; }
        public int Interaktion { get; set; }
        public int Opførsel { get; set; }

        public int Anbefaling { get; set; }

        public string FeedbackGood { get; set; }
        public string FeedbackBetter { get; set; }


        [DefaultValue(false)]
        public bool BasedOnFacebook { get; set; }

        [DefaultValue(false)]
        public bool BasedOnInstagram { get; set; }

        [DefaultValue(false)]
        public bool BasedOnYoutube { get; set; }

        [DefaultValue(false)]
        public bool BasedOnTwitter { get; set; }

        [DefaultValue(false)]
        public bool BasedOnTwitch { get; set; }

        [DefaultValue(false)]
        public bool BasedOnSnapchat { get; set; }

        [DefaultValue(false)]
        public bool BasedOnWebsite { get; set; }


        [DefaultValue(null)]
        public string Answer { get; set; }

        [DefaultValue(false)]
        public bool IsRead { get; set; }

        [DefaultValue(false)]
        public bool IsAnswerRead { get; set; }

    }
}
