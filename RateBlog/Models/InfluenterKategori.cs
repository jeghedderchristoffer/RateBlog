using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class InfluenterKategori
    {


        public int InfluenterId { get; set; }
        public Influenter Influenter { get; set; }


        public int KategoriId { get; set; }
        public Kategori Kategori { get; set; }
    }
}
