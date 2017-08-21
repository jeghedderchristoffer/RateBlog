using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.AccountViewModels
{
    public class RegisterConfirmationViewModel
    {
        [Required(ErrorMessage = "Du skal udfylde din email")]
        [EmailAddress(ErrorMessage = "Det skal være en gyldig mail")]
        public string Email { get; set; }

        [Required( ErrorMessage = "Du skal udfylde dit navn")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Du skal udfylde din fødselsdato")]
        public DateTime? Birthday { get; set; }

        [Required(ErrorMessage = "Du skal udfylde dit postnummer ")]
        [Range(1000, 9999, ErrorMessage = "Du skal vælge et gyldigt postnummer")]
        public int? Postnummer { get; set; }

        [Required(ErrorMessage = "Du skal udfylde dit kodeord")]
        [StringLength(100, ErrorMessage = "Dit kodeord skal være på mindst 6 bogstaver/tal", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Kodeord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bekræft kodeord")]
        [Compare("Password", ErrorMessage = "Kodeordene passer ikke sammen")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Du skal vælge dit køn")]
        [Display(Name = "Køn")]
        public string Gender { get; set; }

        [Required]
        [Compare("isTrue", ErrorMessage = "Du skal godkende betingelserne")]
        public bool AcceptTermsAndConditions { get; set; }

        public bool isTrue
        { get { return true; } }


        [DefaultValue(false)]
        public bool NewLetter { get; set; } 
    }
}
 