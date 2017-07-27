using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.EkspertViewModels
{
    public class EkspertRatingViewModel
    {
        public Influenter Influenter { get; set; }

        [Required]
        public string Offentligfeedback { get; set; }
        [Required]
        public string OffentligfeedbackString { get; set; }

        [Required]
        public int Kvalitet { get; set; }
        [Required]
        public string KvalitetString { get; set; }

        [Required]
        public int Troværdighed { get; set; }
        [Required]
        public string TroværdighedString { get; set; }

        [Required]
        public int Opførsel { get; set; }
        [Required]
        public string OpførselString { get; set; }

        [Required]
        public int Interaktion { get; set; }
        [Required]
        public string InteraktionString { get; set; }

        [Required]
        public bool? Anbefaling { get; set; }
        [Required]
        public string AnbefalingString { get; set; }
        
    }
}
