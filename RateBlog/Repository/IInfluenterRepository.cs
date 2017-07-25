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

        /// <summary>
        /// Sort influenters (ApplicationUsers)
        /// </summary>
        /// <param name="platformIds"></param>
        /// <param name="kategoriIds"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        List<ApplicationUser> SortInfluencerByPlatAndKat(int[] platformIds, int[] kategoriIds, List<ApplicationUser> users); 
    }
}
