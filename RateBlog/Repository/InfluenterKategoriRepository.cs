using RateBlog.Data;
using RateBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Repository
{
    public class InfluenterKategoriRepository : IInfluenterKategoriRepository
    {
        private ApplicationDbContext _dbContext;
        private IKategoriRepository _kategoriRepo; 

        public InfluenterKategoriRepository(ApplicationDbContext dbContext, IKategoriRepository kategoriRepo)
        {
            _dbContext = dbContext;
            _kategoriRepo = kategoriRepo; 
        }

        public bool IsKategoriSelected(int influenterId, int kategoriId)
        {
            if(_dbContext.InfluenterKategori.Any(x => x.InfluenterId == influenterId && x.KategoriId == kategoriId))
            {
                return true; 
            }
            return false;
        }

        public void Insert(int influenterId, int kategoriId, bool isSelected)
        {
            InfluenterKategori ik = new InfluenterKategori()
            {
                InfluenterId = influenterId,
                KategoriId = kategoriId
            };

            if (_dbContext.InfluenterKategori.Any(x => x.InfluenterId == influenterId && x.KategoriId == kategoriId))
            {
                if (!isSelected)
                {
                    _dbContext.Remove(ik);
                    _dbContext.SaveChanges();
                }
            }
            else
            {
                if (isSelected)
                {
                    _dbContext.Add(ik);
                    _dbContext.SaveChanges();
                }
            }
        }

        public List<string> GetAllKategori(int influenterId)
        {
            if (_dbContext.InfluenterKategori.Any(x => x.InfluenterId == influenterId))
            {
                //return _kategoriRepo.GetAll().SingleOrDefault(x => x.KategoriId == kategoriId).KategoriNavn; 
                var list = new List<string>(); 

                foreach(var v in _dbContext.InfluenterKategori.Where(x=> x.InfluenterId == influenterId))
                {
                    list.Add(_kategoriRepo.Get(v.KategoriId).KategoriNavn);
                }

                return list;
            }

            return null; 
        }
    }
}
