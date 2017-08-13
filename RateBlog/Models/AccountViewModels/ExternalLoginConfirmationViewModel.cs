using RateBlog.Models.ManageViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.AccountViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required (ErrorMessage = "Du skal udfylde din email")]
        [EmailAddress (ErrorMessage = "Du skal skrive en gyldig mail")]
        public string Email { get; set; }

        [Required (ErrorMessage = "Du skal udfylde dit navn")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Du skal udfylde din årgang")]
        [Range(1900, 2017, ErrorMessage = "Du skal vælge det år, som du er født i")]
        public int? Year { get; set; }

        [Required(ErrorMessage = "Du skal udfylde dit postnummer ")]
        [Range(1000, 9999, ErrorMessage = "Du skal vælge et gyldigt postnummer")]
        public int? Postnummer { get; set; }

        [Required (ErrorMessage = "Du skal vælge dit køn")]
        [Display(Name = "Køn")]
        public string Gender { get; set; } 

    }
}
