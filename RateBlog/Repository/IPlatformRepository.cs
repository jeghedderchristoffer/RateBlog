using RateBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Repository
{
    public interface IPlatformRepository
    {
        /// <summary>
        /// Get a platform object with a platform id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Platform Get(int Id);

        /// <summary>
        /// Get all platforms
        /// </summary>
        /// <returns></returns>
        List<Platform> GetAll();

        /// <summary>
        /// Get all the InfluenterPlatform objects for a specific influenter. 
        /// </summary>
        /// <param name="inluenterId"></param>
        /// <returns></returns>
        List<InfluenterPlatform> GetAllInfluenterPlatformForInfluenter(int inluenterId);

        /// <summary>
        /// Gets the link for a specific influenter with a specific platform
        /// </summary>
        /// <param name="influenterId"></param>
        /// <param name="platformId"></param>
        /// <returns></returns>
        string GetLink(int influenterId, int platformId);

        /// <summary>
        /// Inserts the InfluenterPlatform object in the database. If the object exist, its either deletes it from the db, or updates it, depending on wether the link is empty or changed.
        /// </summary>
        /// <param name="influenterId"></param>
        /// <param name="platformId"></param>
        /// <param name="link"></param>
        void Insert(int influenterId, int platformId, string link);


    }
}
