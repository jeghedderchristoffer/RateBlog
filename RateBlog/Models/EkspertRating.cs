using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class EkspertRating
    {
        public int Id { get; set; }

        public int Kvalitet { get; set; }
        public string KvalitetString { get; set; }

        public int Troværdighed { get; set; }
        public string TroværdighedString { get; set; }

        public int Interaktion { get; set; }
        public string InteraktionString { get; set; }   

        public int Opførsel { get; set; }
        public string OpførselString { get; set; }

        public bool? Anbefaling { get; set; }
        public string AnbefalingString { get; set; }

        public string OffentligFeedback { get; set; }
        public string OffentligFeedbackString { get; set; }

        public string BestfluenceFeedback { get; set; }
        public string BestfluenceFeedbackString { get; set; }

        public DateTime RateDateTime { get; set; }  

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int InfluenterId { get; set; }
        public Influenter Influenter { get; set; }

    }
}
