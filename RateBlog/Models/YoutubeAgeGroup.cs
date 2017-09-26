using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Bestfluence.Data;

namespace Bestfluence.Models
{
    public class YoutubeAgeGroup : BaseEntity
    {
        public string YoutubeDataId { get; set; }
        public virtual YoutubeData YoutubeData { get; set; }

        [DefaultValue(0)]
        public int ThirteenToSeventeen { get; set; }

        [DefaultValue(0)]
        public int EighteenTothirtyfour { get; set; }

        [DefaultValue(0)]
        public int ThirtyfiveTofortyfour { get; set; }

        [DefaultValue(0)]
        public int FortyFiveTofiftyfive { get; set; }

        [DefaultValue(0)]
        public int FiftyfiveToSixtyfive { get; set; }

        [DefaultValue(0)]
        public int SixtyfivePlus { get; set; }

    }
}
