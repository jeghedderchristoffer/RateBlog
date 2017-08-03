using RateBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Helper
{
    public class SearchHelpModel
    {
        public List<Influenter> InfluencerList { get; set; }
        public List<Kategori> KategoriList { get; set; }
        public List<Platform> PlatformList { get; set; }
    }
}
