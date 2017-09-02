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
        /// Return false or true, if true = wholestars, false = halfstar
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        List<bool> GetStars(double value);

        /// <summary>
        /// Gets influencers total score
        /// </summary>
        /// <param name="id">Influencer ID</param>
        /// <returns></returns>
        double GetTotalScore(string id);

        /// <summary>
        /// Gets single feedback score
        /// </summary>
        /// <param name="id">Influencer ID</param>
        /// <returns></returns>
        double GetSingleScore(string id);

        /// <summary>
        /// Gets the amount of feedback given or recieved. Bool for isInfluencer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int GetFeedbackCount(string id, bool isInfluencer);

        /// <summary>
        /// Gets the time about last feedback to that person
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        double FeedbackCountdown(string userId, string influencerId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int UnreadFeedbackCount(string id);

        int ReadFeedback(string id, string userId);

    }
}
