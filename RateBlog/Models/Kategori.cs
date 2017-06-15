using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class Kategori
    {
        public int KategoriId { get; set; }
        public string KategoriNavn { get; set; }

        public virtual ICollection<InfluenterKategori> InfluenterKategori { get; set; }

    }
}
