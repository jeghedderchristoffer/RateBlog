using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.ManageViewModels
{
    public class InfluencerViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }

        [Required (ErrorMessage = "Du skal angive dit alias")]
        public string Alias { get; set; }

        // Platform/link
        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public string SnapchatLink { get; set; }
        public string YoutubeLink { get; set; }
        public string WebsiteLink { get; set; }
        public string TwitterLink { get; set; }
        public string TwitchLink { get; set; }

        public List<InfluenterKategoriViewModel> IKList { get; set; }

    }
}
