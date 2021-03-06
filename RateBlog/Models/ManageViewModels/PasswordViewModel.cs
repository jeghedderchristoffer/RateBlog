﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models.ManageViewModels
{
    public class PasswordViewModel
    {
        [Required(ErrorMessage = "Du skal udfylde dit gamle kodeord.")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Du skal udfylde dit nye kodeord.")]
        [StringLength(100, ErrorMessage = "Kodeordet skal være mindst 6 bogstaver/tal", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "De 2 kodeord passer ikke.")]
        public string ConfirmPassword { get; set; }
    }
}
