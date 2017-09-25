using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models.ManageViewModels
{
    public class EditViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Du skal udfylde dit navn.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Du skal udfylde din email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Du skal udfylde din fødselsdato")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Birthday { get; set; }

        [Required(ErrorMessage = "Du skal udfylde dit postnummer")]
        [Range(1000, 9999, ErrorMessage = "Du skal vælge et gyldigt postnummer")]
        public int? Postnummer { get; set; }

        [Required(ErrorMessage = "Du skal vælge dit køn")]
        [Display(Name = "Køn")]
        public string Gender { get; set; }

        public IFormFile ProfilePic { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ProfilePic != null)
            {
                if (ProfilePic.ContentType != "image/png" && ProfilePic.ContentType != "image/jpeg")
                {
                    yield return new ValidationResult("Billedet skal være af typen JPEG eller PGN.");
                }

                if (ProfilePic.Length > 1000000)
                {
                    yield return new ValidationResult("Billedet må ikke overstige 1MB.");
                }
            }

        }
    }
}
