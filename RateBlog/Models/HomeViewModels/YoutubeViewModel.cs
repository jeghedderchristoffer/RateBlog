using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models.HomeViewModels
{
    public class YoutubeViewModel
    {

        public YoutubeData YoutubeData { get; set; }

        public int Engagement { get; set; }
        public int Views { get; set; }
        public double MaleViews { get; set; }
        public double FemaleViews { get; set; }
        public int Subcribers { get; set; }

        public int Likes { get; set; }
        public int Dislike { get; set; }
        public int Comments { get; set; }
    }
}
