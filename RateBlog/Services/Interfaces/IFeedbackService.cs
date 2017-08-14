using RateBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Services
{
    public interface IFeedbackService
    {
        /// <summary>
        /// Does the influencer have feedback? Return true if exist, otherwise return false. 
        /// </summary>
        /// <param name="id">Influencer ID</param>
        /// <returns></returns>
        bool HasFeedback(string id);

        /// <summary>
        /// Gets the overall average for a influencer. Rounded up or down depending on the decimals.
        /// </summary>
        /// <param name="id">Influencer ID</param>
        /// <returns></returns>
        int GetAverageFeedbackScore(string id);

        /// <summary>
        /// Gets a single feedback score average. Takes an feedback id as paramenter.
        /// </summary>
        /// <param name="id">Feedback ID</param>
        /// <returns></returns>
        int GetSingleFeedbackScoreAverage(string id);

        /// <summary>
        /// Gets the number of unread feedbacks (as an influencer). 
        /// </summary>
        /// <param name="id">Influencer ID</param>
        /// <returns></returns> 
        int GetUnreadFeedbackCount(string id);

        /// <summary>
        /// Gets the number of unread answers (as an normal user).
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns></returns>
        int GetUnreadAnswerCount(string id);

        /// <summary>
        /// Gets all feedback for an influencer. 
        /// </summary>
        /// <param name="id">Influencer ID</param>
        /// <returns></returns>
        IEnumerable<Feedback> GetAllFeedbackByInfluencer(string id);

        /// <summary>
        /// Gets the amount of feedbacks a influencer have. If you have 4 feedbacks, 4 is returned. 
        /// </summary>
        /// <param name="id">Influencer ID</param>
        /// <returns></returns>
        int GetInfluencerFeedbackCount(string id);

        /// <summary>
        /// Gets the amount of feedbacks you have given to influencers. 
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns></returns>
        int GetUserFeedbackCount(string id);

        // Skipper de 2 næste: GetMyRatingAnswerNumber, GetInfluenterAnswerNumber

        /// <summary>
        /// Gets the amount of answers a user have gotten
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns></returns>
        int GetUserFeedbackAnswersCount(string id);

        /// <summary>
        /// Gets the amount of answers a influencer have given.
        /// </summary>
        /// <param name="id">Influencer ID</param>
        /// <returns></returns>
        int GetInfluencerFeedbackAnswersCount(string id); 

        /// <summary>
        /// Gets the feedback answer percentage for a influencer
        /// </summary>
        /// <param name="id">Influencer ID</param>
        /// <returns></returns>
        double GetInfluencerAnswerPercentage(string id);

        /// <summary>
        /// Gets the hours before you can rate the same influencer again...
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="influenterId">Influencer ID</param>
        /// <returns></returns>
        double GetHoursLeftToRate(string userId, string influenterId);

        /// <summary>
        /// Gets all the feedback for a User.
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns></returns>
        IEnumerable<Feedback> GetAllFeedbackByUser(string id);

        /// <summary>
        /// Gets the last 3 feedbacks for a User
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns></returns>
        IEnumerable<Feedback> GetLast3Feedback(string id);

        /// <summary>
        /// Gets the total score for a influencer. 
        /// </summary>
        /// <param name="id">Influencer ID</param>
        /// <returns></returns>
        double GetTotalScore(string id); 


    }
}
