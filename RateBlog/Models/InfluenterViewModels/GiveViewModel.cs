using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.InfluenterViewModels
{
    public class GiveViewModel
    {
        [Required (ErrorMessage = "Du skal udfylde Kvalitet")]
        [Range(1, 5)]
        public int Kvalitet { get; set; }

        [Required (ErrorMessage = "Du skal udfylde Troværdighed")]
        [Range(1, 5)]
        public int Troværdighed { get; set; }

        [Required(ErrorMessage = "Du skal udfylde Opførsel")]
        [Range(1, 5)]
        public int Opførsel { get; set; }

        [Required (ErrorMessage = "Du skal udfylde Interaktion")]
        [Range(1, 5)]
        public int Interaktion { get; set; }

        [Required (ErrorMessage = "Du skal udfylde Anbefaling")]
        [Range(1, 10)]
        public int Anbefaling { get; set; }

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

        public string FeedbackGood { get; set; }

        public string FeedbackBetter { get; set; }

        public ApplicationUser Follower { get; set; }

        public Influencer Influencer { get; set; } 

    }
}
