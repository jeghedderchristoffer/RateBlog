using Bestfluence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Bestfluence.Services.Interfaces
{
    public interface IAdminService
    {
        /// <summary>
        /// Filters a feedback with respect to a platform.
        /// Given that there are no other filters conserning feedback-
        /// the "FeedbackAllreadyFilterd" must allways be passed with "null"
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Platform"></param>
        /// <param name="FeedbackAllreadyFilterd"></param>
        /// <returns></returns>
        IEnumerable<Feedback> GetPlatformFilterdFeedbacks(string Id, List<int> Platform, IEnumerable<Feedback> FeedbackAllreadyFilterd);

        /// <summary>
        /// Filters a User with respect to a defined Agegroup.
        /// Takes another allready filterd User, or if
        /// it is the first filter takes a "null"
        /// </summary>
        /// <param name="AgeGroup"></param>
        /// <param name="UsersAllreadyFilterd"></param>
        /// <returns></returns>
        IEnumerable<ApplicationUser> GetAgeGroupFilterdUsers(List<int> AgeGroup, IEnumerable<ApplicationUser> UsersAllreadyFilterd);

        /// <summary>
        /// Filters a User with respect to their Gender.
        /// Takes another allready filterd User, or if
        /// it is the first filter takes a "null"
        /// the køn parameters can be "female", "male" and "both"
        /// </summary>
        /// <param name="Køn"></param>
        /// <param name="UsersAllreadyFilterd"></param>
        /// <returns></returns>
        IEnumerable<ApplicationUser> GetGenderFilterdUsers(string Køn, IEnumerable<ApplicationUser> UsersAllreadyFilterd);

        /// <summary>
        /// The Actual filter. compares feedback ApplicationId
        /// With users ApplicationId and joins. Returns the final filterd Feedbacks
        /// </summary>
        /// <param name="FeedbackAllreadyFilterd"></param>
        /// <param name="UsersAllreadyFilterd"></param>
        /// <returns></returns>
        IEnumerable<Feedback> GetJoinFiltersAndGetFeedbacks(IEnumerable<Feedback> FeedbackAllreadyFilterd, IEnumerable<ApplicationUser> UsersAllreadyFilterd);

        /// <summary>
        /// Used to join all filters in one method.
        /// And to get them in the prefferd sequence
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Platform"></param>
        /// <param name="AldersGruppe"></param>
        /// <param name="Køn"></param>
        /// <returns></returns>
        IEnumerable<Feedback> GetTheFilterdFeedbacks(string Id, List<int> Platform, List<int> AldersGruppe, string Køn);

        /// <summary>
        /// Perform a distinct on id of the feedback
        /// Removes all ApplicationId's which is the same
        /// </summary>
        /// <param name="FeedbackAllreadyFilterd"></param>
        /// <returns></returns>
        double GetFilterdUniqueUsers(IEnumerable<Feedback> FeedbackAllreadyFilterd);
    
        /// <summary>
        /// Gets the average kavalitet
        /// </summary>
        /// <param name="FilterdFeedback"></param>
        /// <returns></returns>
        double GetAverageKvalitetForFiltered(IEnumerable<Feedback> FilterdFeedback);

        /// <summary>
        /// Gets the average Opførsel
        /// </summary>
        /// <param name="FilterdFeedback"></param>
        /// <returns></returns>
        double GetAverageOpførelseForFiltered(IEnumerable<Feedback> FilterdFeedback);

        /// <summary>
        /// Gets the average Troværdighed
        /// </summary>
        /// <param name="FilterdFeedback"></param>
        /// <returns></returns>
        double GetAverageTroværdighedForFiltered(IEnumerable<Feedback> FilterdFeedback);

        /// <summary>
        /// Gets the average Interaktion
        /// </summary>
        /// <param name="FilterdFeedback"></param>
        /// <returns></returns>
        double GetAverageInteraktionForFiltered(IEnumerable<Feedback> FilterdFeedback);

        /// <summary>
        /// Gets the number of feedbacks
        /// </summary>
        /// <param name="FilterdFeedback"></param>
        /// <returns></returns>
        double GetNumberFeedback(IEnumerable<Feedback> FilterdFeedback);

        /// <summary>
        /// Gets nps for the feedbacks
        /// </summary>
        /// <param name="FilterdFeedback"></param>
        /// <returns></returns>
        double GetNpsForFilter(IEnumerable<Feedback> FilterdFeedback);


        List<int> GetPlatformToDisplay(string Id);

        List<int> GetAgeToDisplay(string Id);

        List<int> GetGenderToDisplay(string Id);




        double GetNumberTotalAvg7daysOrProcent(string Id, bool Get7Days, int days);

        double GetNumberRatingDifferenceProcentage7days(string Id, int days);

        double GetNumberKvalitetDifference7days(string Id, bool GetDifference, int days);

        double GetNumberTroværdighedDifference7days(string Id, bool GetDifference, int days);

        double GetNumberInteraktionDifference7days(string Id, bool GetDifference, int days);

        double GetNumberOpførselDifference7days(string Id, bool GetDifference, int days);

        IEnumerable<Feedback>[] GetReturningAndUniqueFeedbacks(IEnumerable<Feedback> DaysFilterdFeedback);

        IEnumerable<Feedback>[] GetFeedbackDifferenceInRatingLast7Days(string Id, int days);

        double UniqeUsersComparedToDays(string Id, int days, int timeGroup);

        double ReturningUsersComparedToDays(string Id, int days, int timeGroup);

    }
}

