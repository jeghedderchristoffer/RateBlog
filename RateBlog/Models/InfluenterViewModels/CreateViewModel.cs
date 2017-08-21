using Microsoft.AspNetCore.Http;
using RateBlog.Models.ManageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.InfluenterViewModels
{
    public class CreateViewModel
    {
        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public string SnapchatLink { get; set; }
        public string YoutubeLink { get; set; }
        public string WebsiteLink { get; set; }
        public string TwitterLink { get; set; }
        public string TwitchLink { get; set; }

        public IFormFile ProfilePic { get; set; }

        public List<InfluenterKategoriViewModel> IKList { get; set; }

        public Influencer Influenter { get; set; }

        public string ProfileText { get; set; } 
    }
}
