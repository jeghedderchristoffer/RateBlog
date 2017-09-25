using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models.ManageViewModels
{
    public class InfluencerViewModel
    {
        [Required(ErrorMessage = "Du skal udfylde dit Alias")]
        public string Alias { get; set; }

        [StringLength(30, MinimumLength = 4, ErrorMessage = "Din URL skal være på minimum 4 karaktere og maximum 30")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Din URL må ikke indeholde mellemrum!")]
        public string Url { get; set; }

        public string ProfileText { get; set; } 

        public IEnumerable<InfluencerCategory> InfluencerCategories { get; set; }

        public string InstagramLink { get; set; }
        public string SnapchatLink { get; set; }
        public string TwitterLink { get; set; }

        public string TwitchLink { get; set; }
        public string FacebookLink { get; set; }
        public string WebsiteLink { get; set; }
        public string YoutubeLink { get; set; }
        public string SecondYoutubeLink { get; set; }

    } 
}
