using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BestfluenceBusiness.Models.CampaignsViewModel
{
    public class Step1
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public bool Facebook { get; set; }

        [Required]
        public bool Youtube { get; set; }

        [Required]
        public bool Instagram { get; set; }

        [Required]
        public int InfluencerNumber { get; set; }
    }
}
