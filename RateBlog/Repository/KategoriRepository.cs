using RateBlog.Data;
using RateBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Repository
{
    public class KategoriRepository : IKategoriRepository
    {

        private ApplicationDbContext _applicationDbContext;

        public KategoriRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

        }

        public Kategori Get(int Id)
        {

            return _applicationDbContext.Kategori.FirstOrDefault(x => x.KategoriId == Id);

          
        }

        public List<Kategori> GetAll()
        {
            return _applicationDbContext.Kategori.ToList();
        }

    }
}
