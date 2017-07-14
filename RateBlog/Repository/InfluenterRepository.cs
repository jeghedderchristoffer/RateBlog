using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RateBlog.Models;
using RateBlog.Data;
using Microsoft.EntityFrameworkCore;

namespace RateBlog.Repository
{
    public class InfluenterRepository : IInfluenterRepository
    {

        private ApplicationDbContext _applicationDbContext;

        public InfluenterRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

        }

        public void Delete(int InfluenterId)
        {
            Influenter influenter = _applicationDbContext.Influenter.Find(InfluenterId);
            _applicationDbContext.Influenter.Remove(influenter);
            _applicationDbContext.SaveChanges();
        }

        public Influenter Get(int Id)
        {
            return _applicationDbContext.Influenter.FirstOrDefault(x => x.InfluenterId == Id);
        }

        public List<Influenter> GetAll()
        {
            return _applicationDbContext.Influenter.ToList();
        }

        public void Update(Influenter influenter)
        {
            _applicationDbContext.Entry(influenter).State = EntityState.Modified;
            _applicationDbContext.SaveChanges();
        }

        public void Add(Influenter influenter)
        {
            _applicationDbContext.Influenter.Add(influenter);
            _applicationDbContext.SaveChanges();           
        }



        public IQueryable<int> GetAllInfluentersForPlatforms(int[] platformIds)
        {
            return _applicationDbContext.InfluenterPlatform.Where(x => platformIds.Contains(x.PlatformId)).Select(x => x.InfluenterId);
        }

        public IQueryable<int> GetAllInfluentersForKategori(int[] kategoriIds)
        {
            return _applicationDbContext.InfluenterKategori.Where(x => kategoriIds.Contains(x.KategoriId)).Select(x => x.InfluenterId);
        }


    }
}
