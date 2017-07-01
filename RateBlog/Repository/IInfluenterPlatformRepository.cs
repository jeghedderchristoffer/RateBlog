using RateBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Repository
{
    public interface IInfluenterPlatformRepository
    {
        void Insert(int influenterId, int platformId, string link);
        string GetLink(int influenterId, int platformId); 
        List<InfluenterPlatform> GetAllByInfluenter(int inluenterId);
    }
}
