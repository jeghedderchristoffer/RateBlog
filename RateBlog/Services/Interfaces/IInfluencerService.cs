using RateBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Services.Interfaces
{
    public interface IInfluencerService
    {
        /// <summary>
        /// Gets all influencers by search for their Alias. 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        IEnumerable<Influencer> GetAll(string search);

        Task<Influencer> GetInfluecerAsync(string id);

        IEnumerable<Influencer> GetUnApprovedInfluencers();

        bool IsInfluencer(string id); 

    }
}
