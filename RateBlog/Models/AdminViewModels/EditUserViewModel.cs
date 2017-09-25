using Bestfluence.Models.ManageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models.AdminViewModels
{
    public class EditUserViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public Influencer Influencer { get; set; }

        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public string SnapchatLink { get; set; }
        public string YoutubeLink { get; set; }
        public string SecondYoutubeLink { get; set; } 
        public string WebsiteLink { get; set; }
        public string TwitterLink { get; set; }
        public string TwitchLink { get; set; }
    }
}
