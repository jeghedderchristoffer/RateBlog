using RateBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Repository
{
    public interface IRatingRepository
    {
        /// <summary>
        /// Get single rating object by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Rating Get(int Id);

        /// <summary>
        /// Get all rating objects
        /// </summary>
        /// <returns></returns>
        List<Rating> GetAll();

        /// <summary>
        /// Add a rating object
        /// </summary>
        /// <param name="rating"></param>
        void Add(Rating rating);

        /// <summary>
        /// Update an existing rating object
        /// </summary>
        /// <param name="rating"></param>
        void Update(Rating rating);

        /// <summary>
        /// Delete a rating object
        /// </summary>
        /// <param name="RatingId"></param>
        void Delete(int RatingId);

        /// <summary>
        /// Add a InfuenterPlatform object. IMPORTANT: Not a rating object, but a InfluenterRating object!
        /// </summary>
        /// <param name="influenterId"></param>
        /// <param name="ratingId"></param>
        void AddInfluenterPlatform(int influenterId, int ratingId); 
    }
}
