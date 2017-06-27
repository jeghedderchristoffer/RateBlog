using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.ManageViewModels
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Du skal udfylde dit navn.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Du skal udfylde din email.")]
        public string Email { get; set; }

        public DateTime? Birth { get; set; }

        public string City { get; set; }

        public string ProfileText { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsInfluenter { get; set; }

        // Platform/link
        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public string SnapchatLink { get; set; }
        public string YoutubeLink { get; set; }
        public string WebsiteLink { get; set; }
        public string TwitterLink { get; set; }
        public string TwitchLink { get; set; }

        // Kategori
        public bool LifestyleBool { get; set; }
        public bool DIYBool { get; set; }
        public bool VlogBool { get; set; }
        public bool BeautyBool { get; set; }
        public bool GamingBool { get; set; }
        public bool EntertainmentBool { get; set; }
        public bool FoodBool { get; set; }
        public bool FashionBool { get; set; }
        public bool MommyBool { get; set; }
         
        public List<InfluenterKategoriViewModel> IKList { get; set; }

        public Influenter Influenter { get; set; } 


    }
}
