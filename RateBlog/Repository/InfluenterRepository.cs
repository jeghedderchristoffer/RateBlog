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

        public List<ApplicationUser> SortInfluencerByPlatAndKat(int[] platformIds, int[] kategoriIds, List<ApplicationUser> users)
        {
            var influenterIds = new List<int>();
            var resultUserList = new List<ApplicationUser>();

            var resultKat = new List<InfluenterKategori>();
            var resultPlat = new List<InfluenterPlatform>(); 

            foreach (var v in users)
            {
                influenterIds.Add(v.InfluenterId.Value);
            }

            if(platformIds.Count() == 0 && kategoriIds.Count() == 0)
            {
                return users; 
            }

            if (platformIds.Count() != 0)
            {
                var influenterPlatform = _applicationDbContext.InfluenterPlatform.Where(x => platformIds.Contains(x.PlatformId) && influenterIds.Contains(x.InfluenterId)).ToList();
                resultPlat = influenterPlatform.GroupBy(x => x.InfluenterId).Where(p => p.Count() >= platformIds.Count()).SelectMany(x => x).ToList();

                foreach (var v in resultPlat)
                {
                    var user = _applicationDbContext.Users.SingleOrDefault(x => x.InfluenterId == v.InfluenterId);
                    if (!resultUserList.Contains(user))
                    {
                        resultUserList.Add(user);
                    }
                }
            }
            if (kategoriIds.Count() != 0)
            {
                var influenterKategori = _applicationDbContext.InfluenterKategori.Where(x => kategoriIds.Contains(x.KategoriId) && influenterIds.Contains(x.InfluenterId)).ToList();
                resultKat = influenterKategori.GroupBy(x => x.InfluenterId).Where(p => p.Count() >= kategoriIds.Count()).SelectMany(x => x).ToList();
              
                foreach (var v in resultKat)
                {
                    var user = _applicationDbContext.Users.SingleOrDefault(x => x.InfluenterId == v.InfluenterId);
                    if (!resultUserList.Contains(user))
                    {
                        resultUserList.Add(user);
                    }
                }
            }

            var endList = new List<ApplicationUser>(); 

            foreach(var user in resultUserList)
            {
                if(kategoriIds.Count() == 0 || platformIds.Count() == 0)
                {
                    return resultUserList; 
                }

                if(resultKat.Any(x => x.InfluenterId == user.InfluenterId) && resultPlat.Any(p => p.InfluenterId == user.InfluenterId))
                {
                    endList.Add(user); 
                }
            }

            return endList;



        }

        public ApplicationUser GetByStringID(string id)
        {

            return _applicationDbContext.ApplicationUser.FirstOrDefault(x => x.Id == id);
        }
    }
}
