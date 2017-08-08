using Microsoft.AspNetCore.Identity;
using RateBlog.Models;
using RateBlog.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RateBlog.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IRepository<Feedback> _feedbackRepo;
        private readonly IRepository<ExpertFeedback> _expertFeedbackRepo;
        private readonly UserManager<ApplicationUser> _userManager; 

        public FeedbackService(IRepository<Feedback> feedbackRepo, IRepository<ExpertFeedback> expertFeedbackRepo, UserManager<ApplicationUser> userManager)
        {
            _feedbackRepo = feedbackRepo;
            _expertFeedbackRepo = expertFeedbackRepo;
            _userManager = userManager; 
        }

        public IEnumerable<ExpertFeedback> GetAllExpertFeedbackByInfluencer(int id)
        {
            return _expertFeedbackRepo.GetAll().Where(x => x.InfluenterId == id);
        }

        public IEnumerable<Feedback> GetAllFeedbackByInfluencer(int id)
        {
            return _feedbackRepo.GetAll().Where(x => x.InfluenterId == id);
        }

        public IEnumerable<Feedback> GetAllFeedbackByUser(string id)
        {
            return _feedbackRepo.GetAll().Where(x => x.ApplicationUserId == id);
        }

        public IEnumerable<ExpertFeedback> GetAllFeedbackForExpert(string id)
        {
            return _expertFeedbackRepo.GetAll().Where(x => x.ApplicationUserId == id);
        }

        public int GetAverageExpertFeedbackScore(int id)
        {
            if (_expertFeedbackRepo.GetAll().Any(x => x.InfluenterId == id))
            {
                var feedbacks = _expertFeedbackRepo.GetAll().Where(x => x.InfluenterId == id);
                int numberOfFeedbacks = 0;
                double allFeedbackSums = 0;

                foreach (var f in feedbacks)
                {
                    double feedbackSum = 0;

                    // Tager alle værdier, plusser dem sammen og dividere dem med antallet af ratings == gennemsnit
                    var feedback = _feedbackRepo.Get(f.Id);

                    feedbackSum += feedback.Interaktion;
                    feedbackSum += feedback.Opførsel;
                    feedbackSum += feedback.Troværdighed;
                    feedbackSum += feedback.Kvalitet;
                    feedbackSum = feedbackSum / 4;

                    // Antal ratings
                    numberOfFeedbacks++;

                    // Tilføjer dem til samlingen
                    allFeedbackSums += feedbackSum;
                }

                double average = (allFeedbackSums / numberOfFeedbacks);

                return (int)Math.Round(average, 0, MidpointRounding.AwayFromZero);
            }

            return 0;
        }

        public int GetAverageFeedbackScore(int id)
        {
            if (_feedbackRepo.GetAll().Any(x => x.InfluenterId == id))
            {
                var feedbacks = _feedbackRepo.GetAll().Where(x => x.InfluenterId == id);
                int numberOfFeedbacks = 0;
                double allFeedbackSums = 0;

                foreach (var f in feedbacks)
                {
                    double feedbackSum = 0;

                    // Tager alle værdier, plusser dem sammen og dividere dem med antallet af ratings == gennemsnit
                    var feedback = _feedbackRepo.Get(f.Id);

                    feedbackSum += feedback.Interaktion;
                    feedbackSum += feedback.Opførsel;
                    feedbackSum += feedback.Troværdighed;
                    feedbackSum += feedback.Kvalitet;
                    feedbackSum = feedbackSum / 4;

                    // Antal ratings
                    numberOfFeedbacks++;

                    // Tilføjer dem til samlingen
                    allFeedbackSums += feedbackSum;
                }

                double average = (allFeedbackSums / numberOfFeedbacks);

                return (int)Math.Round(average, 0, MidpointRounding.AwayFromZero);
            }

            return 0;
        }

        public int GetExpertFeedbackCount(string id)
        {
            if (_expertFeedbackRepo.GetAll().Any(x => x.ApplicationUserId == id))
            {
                return _expertFeedbackRepo.GetAll().Where(x => x.ApplicationUserId == id).Count();
            }
            return 0;
        }

        public int GetExpertFeedbackCountForInfluencer(int id)
        {
            try
            {
                return _expertFeedbackRepo.GetAll().Where(x => x.InfluenterId == id).Count();
            }
            catch (ArgumentNullException)
            {
                return 0;
            }
        }

        public double GetHoursLeftToRate(string userId, int influenterId)
        {
            if (_feedbackRepo.GetAll().Any(x => x.InfluenterId == influenterId && x.ApplicationUserId == userId))
            {
                var user = _userManager.Users.SingleOrDefault(x => x.Id == userId);
                var feedback = _feedbackRepo.GetAll().Where(x => x.InfluenterId == influenterId && x.ApplicationUserId == userId).OrderByDescending(x => x.RateDateTime).FirstOrDefault();

                var timeSpan = DateTime.Now - feedback.RateDateTime;
                var hours = timeSpan.TotalHours;

                return hours;
            }

            return 0;
        }

        public double GetInfluencerAnswerPercentage(int id)
        {
            if (_feedbackRepo.GetAll().Any(x => x.InfluenterId == id && x.Answer != null))
            {
                var numberOfAnswer = _feedbackRepo.GetAll().Where(x => x.InfluenterId == id && x.Answer != null).Count();
                var numberOfFeedbacks = _feedbackRepo.GetAll().Where(x => x.InfluenterId == id && x.IsRead == true).Count();

                double result = (100.0 / numberOfFeedbacks) * numberOfAnswer;

                return result;

            }
            return 0;
        }

        public int GetInfluencerFeedbackAnswersCount(int id)
        {
            if (_feedbackRepo.GetAll().Any(x => x.InfluenterId == id && x.Answer != null))
            {
                return _feedbackRepo.GetAll().Where(x => x.InfluenterId == id && x.Answer != null).Count();
            }
            return 0;
        }

        public int GetInfluencerFeedbackCount(int id)
        {
            if (_feedbackRepo.GetAll().Any(x => x.InfluenterId == id))
            {
                return _feedbackRepo.GetAll().Where(x => x.InfluenterId == id).Count();
            }
            return 0;
        }

        public IEnumerable<Feedback> GetLast3Feedback(string id)
        {
            return _feedbackRepo.GetAll().Where(x => x.ApplicationUserId == id).OrderByDescending(x => x.RateDateTime).Take(3);
        }

        public int GetSingleExpertFeedbackScoreAverage(int id)
        {
            var feedback = _expertFeedbackRepo.Get(id);
            double feedbackSum = 0;

            // Tager alle værdier, plusser dem sammen og dividere dem med antallet af ratings == gennemsnit
            feedbackSum += feedback.Interaktion;
            feedbackSum += feedback.Opførsel;
            feedbackSum += feedback.Troværdighed;
            feedbackSum += feedback.Kvalitet;
            feedbackSum = feedbackSum / 4;

            return (int)Math.Round(feedbackSum, 0, MidpointRounding.AwayFromZero);
        }

        public int GetSingleFeedbackScoreAverage(int id)
        {
            var feedback = _feedbackRepo.Get(id);
            double feedbackSum = 0;

            // Tager alle værdier, plusser dem sammen og dividere dem med antallet af ratings == gennemsnit
            feedbackSum += feedback.Interaktion;
            feedbackSum += feedback.Opførsel;
            feedbackSum += feedback.Troværdighed;
            feedbackSum += feedback.Kvalitet;
            feedbackSum = feedbackSum / 4;

            return (int)Math.Round(feedbackSum, 0, MidpointRounding.AwayFromZero);
        }

        public double GetTotalScore(int id)
        {
            if (_feedbackRepo.GetAll().Any(x => x.InfluenterId == id))
            {
                var feedbacks = _feedbackRepo.GetAll().Where(x => x.InfluenterId == id);
                int numberOfFeedbacks = 0;
                double allFeedbackSums = 0;

                foreach (var f in feedbacks)
                {
                    double feedbackSum = 0;

                    // Tager alle værdier, plusser dem sammen og dividere dem med antallet af ratings == gennemsnit
                    var feedback = _feedbackRepo.Get(f.Id);

                    feedbackSum += feedback.Interaktion;
                    feedbackSum += feedback.Opførsel;
                    feedbackSum += feedback.Troværdighed;
                    feedbackSum += feedback.Kvalitet;
                    feedbackSum = feedbackSum / 4;

                    // Antal ratings
                    numberOfFeedbacks++;

                    // Tilføjer dem til samlingen
                    allFeedbackSums += feedbackSum;
                }

                double average = (allFeedbackSums / numberOfFeedbacks);

                return Math.Round(average, 1);
            }

            return 0;
        }

        public int GetUnreadAnswerCount(string id)
        {
            var allFeedbacks = _feedbackRepo.GetAll().Where(x => x.ApplicationUserId == id);
            int count = 0;
            foreach (var v in allFeedbacks)
            {
                if (!string.IsNullOrEmpty(v.Answer) && v.IsAnswerRead == false)
                {
                    count++;
                }
            }

            return count;
        }

        public int GetUnreadFeedbackCount(int id)
        {
            var feedbacks = _feedbackRepo.GetAll().Where(x => x.InfluenterId == id);

            int number = 0;

            foreach (var v in feedbacks)
            {
                if (_feedbackRepo.Get(v.Id).IsRead == false)
                {
                    number++;
                }
            }

            return number;
        }

        public int GetUserFeedbackAnswersCount(string id)
        {
            if (_feedbackRepo.GetAll().Any(x => x.ApplicationUserId == id && x.Answer != null))
            {
                return _feedbackRepo.GetAll().Where(x => x.ApplicationUserId == id && x.Answer != null).Count();
            }
            return 0;
        }

        public int GetUserFeedbackCount(string id)
        {
            if (_feedbackRepo.GetAll().Any(x => x.ApplicationUserId == id))
            {
                return _feedbackRepo.GetAll().Where(x => x.ApplicationUserId == id).Count();
            }
            return 0;
        }

        public bool HasExpertFeedback(int id)
        {
            if (_expertFeedbackRepo.GetAll().Any(x => x.InfluenterId == id))
            {
                return true;
            }
            return false;
        }

        public bool HasFeedback(int id)
        {
            if (_feedbackRepo.GetAll().Any(x => x.InfluenterId == id))
            {
                return true;
            }
            return false;
        }
    }
}
