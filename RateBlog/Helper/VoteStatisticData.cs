using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Helper
{
    public class VoteStatisticData
    {
        public int Males { get; set; }
        public int Females { get; set; }
        public int[] AgeGroup { get; set; }
        public IEnumerable<VoteData> AnswerData { get; set; }
        public int AnswerSum { get; set; } 
    }
}
