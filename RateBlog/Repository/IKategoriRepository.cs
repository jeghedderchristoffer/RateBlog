using RateBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Repository
{
    public interface IKategoriRepository
    {
        Kategori Get(int Id);

        List<Kategori> GetAll();

    }
}
