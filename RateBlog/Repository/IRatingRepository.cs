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

        /// <summary>
        /// Get average for single rating
        /// </summary>
        /// <param name="ratingId"></param>
        /// <returns></returns>
        double GetSingleRatingAverage(int ratingId);

        int GetSingleRating(int ratingId, string name);

        /// <summary>
        /// Get the rating number for an influenter!
        /// </summary>
        /// <param name="influenterId"></param>
        /// <returns></returns>
        int GetInfluenterRatingNumber(int influenterId);

        /// <summary>
        /// Gets the answer number for a influencer
        /// </summary>
        /// <param name="influenterId"></param>
        /// <returns></returns>
        int GetInfluenterAnswerNumber(int influenterId); 

        /// <summary>
        /// Get the users rating. If the user has rated 3 people, 3 will be returned. 
        /// </summary>
        /// <param name="applicationUserId"></param>
        /// <returns></returns>
        int GetMyRatingNumber(string applicationUserId);

        /// <summary>
        /// Gets the number of answer the user has recieved from influencers. If the user has rated 5, but only 3 influencer has answered, 3 will be returned
        /// </summary>
        /// <param name="applicationUserId"></param>
        /// <returns></returns>
        int GetMyRatingAnswerNumber(string applicationUserId);

        double GetAnswerPercentageForInfluencer(int influenterId);
    }
}
