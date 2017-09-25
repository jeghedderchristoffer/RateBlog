using Bestfluence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Repository
{
    public interface IInfluencerRepository
    {
        Influencer Get(string id);
        IEnumerable<Influencer> GetAll();
        void Add(Influencer entity);
        void Update(Influencer entity);
        void Delete(Influencer entity);
        void SaveChanges(); 
    }
}
