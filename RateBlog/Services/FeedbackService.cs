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
        private readonly UserManager<ApplicationUser> _userManager; 

        public FeedbackService(IRepository<Feedback> feedbackRepo, UserManager<ApplicationUser> userManager)
        {
            _feedbackRepo = feedbackRepo;
            _userManager = userManager; 
        }

        public IEnumerable<Feedback> GetAllFeedbackByInfluencer(string id)
        {
            return _feedbackRepo.GetAll().Where(x => x.InfluenterId == id);
        }

        public IEnumerable<Feedback> GetAllFeedbackByUser(string id)
        {
            return _feedbackRepo.GetAll().Where(x => x.ApplicationUserId == id);
        }       

        public int GetAverageFeedbackScore(string id)
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

        public double GetHoursLeftToRate(string userId, string influenterId)
        {
            if (_feedbackRepo.GetAll().Any(x => x.InfluenterId == influenterId && x.ApplicationUserId == userId))
            {
                var user = _userManager.Users.SingleOrDefault(x => x.Id == userId);
                var feedback = _feedbackRepo.GetAll().Where(x => x.InfluenterId == influenterId && x.ApplicationUserId == userId).OrderByDescending(x => x.FeedbackDateTime).FirstOrDefault();

                var timeSpan = DateTime.Now - feedback.FeedbackDateTime;
                var hours = timeSpan.TotalHours;

                return hours;
            }

            return 0;
        }

        public double GetInfluencerAnswerPercentage(string id)
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

        public int GetInfluencerFeedbackAnswersCount(string id)
        {
            if (_feedbackRepo.GetAll().Any(x => x.InfluenterId == id && x.Answer != null))
            {
                return _feedbackRepo.GetAll().Where(x => x.InfluenterId == id && x.Answer != null).Count();
            }
            return 0;
        }

        public int GetInfluencerFeedbackCount(string id)
        {
            if (_feedbackRepo.GetAll().Any(x => x.InfluenterId == id))
            {
                return _feedbackRepo.GetAll().Where(x => x.InfluenterId == id).Count();
            }
            return 0;
        }

        public IEnumerable<Feedback> GetLast3Feedback(string id)
        {
            return _feedbackRepo.GetAll().Where(x => x.ApplicationUserId == id).OrderByDescending(x => x.FeedbackDateTime).Take(3);
        }

       

        public int GetSingleFeedbackScoreAverage(string id)
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

        public double GetTotalScore(string id)
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

            return 0.0;
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

        public int GetUnreadFeedbackCount(string id)
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

        public bool HasFeedback(string id)
        {
            if (_feedbackRepo.GetAll().Any(x => x.InfluenterId == id))
            {
                return true;
            }
            return false;
        }
    }
}
