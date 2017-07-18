﻿using RateBlog.Models;
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
        /// Gets the average rating for a influenter
        /// </summary>
        /// <param name="influenterId"></param>
        /// <returns></returns>
        double GetRatingAverage(int influenterId);

        /// <summary>
        /// Does this influenter have ratings?
        /// </summary>
        /// <param name="influenterId"></param>
        /// <returns></returns>
        bool HasRatings(int influenterId);

        /// <summary>
        /// Gets the number of unread ratings.
        /// </summary>
        /// <param name="influenterId"></param>
        /// <returns></returns>
        int GetUnreadRatingsNumber(int influenterId);

        /// <summary>
        /// Get all ratings for a influenter
        /// </summary>
        /// <param name="influenterId"></param>
        /// <returns></returns>
        List<Rating> GetRatingForInfluenter(int influenterId);

        /// <summary>
        /// Count method
        /// </summary>
        /// <param name="influenterId"></param>
        /// <returns></returns>
        int CountRatings(int influenterId); 
    }
}
