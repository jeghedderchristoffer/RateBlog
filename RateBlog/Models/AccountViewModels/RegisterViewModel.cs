using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Missing")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Missing")]
        [EmailAddress(ErrorMessage = "Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Missing")]
        [StringLength(100, ErrorMessage = "Password", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "NoMatch")]
        public string ConfirmPassword { get; set; }
    }
}
