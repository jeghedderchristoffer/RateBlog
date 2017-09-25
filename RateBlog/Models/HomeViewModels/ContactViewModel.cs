using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models.FooterViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Du skal udfylde din email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Du skal udfylde dit navn")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Du skal skrive et emne")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Du skal skrive en besked")]
        public string Text { get; set; }
    }
}
