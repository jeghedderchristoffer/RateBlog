using Bestfluence.Models.ManageViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models.AccountViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required (ErrorMessage = "Du skal udfylde din email")]
        [EmailAddress (ErrorMessage = "Du skal skrive en gyldig mail")]
        public string Email { get; set; }

        [Required (ErrorMessage = "Du skal udfylde dit navn")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Du skal udfylde din fødselsdato")]
        [DataType(DataType.Date, ErrorMessage = "Du skal udfylde en gyldig fødselsdato")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Birthday { get; set; }

        [Required(ErrorMessage = "Du skal udfylde dit postnummer ")]
        [Range(1000, 9999, ErrorMessage = "Du skal vælge et gyldigt postnummer")]
        public int? Postnummer { get; set; }

        public string City { get; set; } 

        [Required (ErrorMessage = "Du skal vælge dit køn")]
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
