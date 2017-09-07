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
        /// Gets the unread feedbackCount
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int UnreadFeedbackCount(string id);

        /// <summary>
        /// Read the feedbacks??
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        int ReadFeedback(string id, string userId);

        /// <summary>
        /// Gets all the unread feedback reports for users. 
        /// </summary>
        /// <returns></returns>
        IEnumerable<FeedbackReport> GetUnreadFeedbackReports();

        /// <summary>
        /// Gets all the reports for 1 feedback
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<FeedbackReport> GetReportForFeedback(string id);
        
        /// <summary>
        /// Gets all the feedback Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Feedback GetFeedbackInfo(string id); 

    }
}
