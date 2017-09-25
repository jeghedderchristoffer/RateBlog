using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Du skal udfylde dit navn")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Du skal udfylde din email")]
        [EmailAddress(ErrorMessage = "Det skal være en gyldig email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
