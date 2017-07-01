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

        public int GetIdByName(string name)
        {
            if(_applicationDbContext.Kategori.Any(x => x.KategoriNavn == name))
            {
                return _applicationDbContext.Kategori.SingleOrDefault(x => x.KategoriNavn == name).KategoriId; 
            }

            return 0;
        }

    
        public List<string> GetInfluentersCategories(int influenterId)
        {
            if (_applicationDbContext.InfluenterKategori.Any(x => x.InfluenterId == influenterId))
            {
                //return _kategoriRepo.GetAll().SingleOrDefault(x => x.KategoriId == kategoriId).KategoriNavn; 
                var list = new List<string>();

                foreach (var v in _applicationDbContext.InfluenterKategori.Where(x => x.InfluenterId == influenterId))
                {
                    list.Add(Get(v.KategoriId).KategoriNavn);
                }

                return list;
            }

            return null;
        }

        public void Insert(int influenterId, int kategoriId, bool isSelected)
        {
            InfluenterKategori ik = new InfluenterKategori()
            {
                InfluenterId = influenterId,
                KategoriId = kategoriId
            };

            if (_applicationDbContext.InfluenterKategori.Any(x => x.InfluenterId == influenterId && x.KategoriId == kategoriId))
            {
                if (!isSelected)
                {
                    _applicationDbContext.Remove(ik);
                    _applicationDbContext.SaveChanges();
                }
            }
            else
            {
                if (isSelected)
                {
                    _applicationDbContext.Add(ik);
                    _applicationDbContext.SaveChanges();
                }
            }
        }

        public bool IsKategoriSelected(int influenterId, int kategoriId)
        {
            if (_applicationDbContext.InfluenterKategori.Any(x => x.InfluenterId == influenterId && x.KategoriId == kategoriId))
            {
                return true;
            }
            return false;
        }
    }
}
