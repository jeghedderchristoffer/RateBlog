﻿using Bestfluence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Helper
{
    public class SearchHelpModel
    {
        public List<Influencer> InfluencerList { get; set; }
        public List<Category> KategoriList { get; set; }
        public List<Platform> PlatformList { get; set; }
    }
}
