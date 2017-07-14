using RateBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Repository
{
    public interface IInfluenterRepository
    {
        Influenter Get(int Id);

        List<Influenter> GetAll();

        void Add(Influenter influenter);

        void Update(Influenter influenter);

        void Delete(int InfluenterId);


        IQueryable<int> GetAllInfluentersForPlatforms(int[] platformIds);

        IQueryable<int> GetAllInfluentersForKategori(int[] kategoriIds);
    }
}
