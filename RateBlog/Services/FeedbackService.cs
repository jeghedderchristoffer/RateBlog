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
        private readonly IInfluencerRepository _influencerRepo;
        private readonly IRepository<Feedback> _feedbackRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public FeedbackService(IInfluencerRepository influencerRepo, IRepository<Feedback> feedbackRepo, UserManager<ApplicationUser> userManager)
        {
            _influencerRepo = influencerRepo;
            _feedbackRepo = feedbackRepo;
            _userManager = userManager;
        }

        public double FeedbackCountdown(string userId, string influencerId)
        {
            var feedback = _feedbackRepo.GetAll().Where(x => x.ApplicationUserId == userId && x.InfluenterId == influencerId);

            if (feedback != null && feedback.Count() != 0)
            {
                var latestFeedback = feedback.OrderByDescending(x => x.FeedbackDateTime).FirstOrDefault();
                var timeSpan = DateTime.Now - latestFeedback.FeedbackDateTime;
                var hours = timeSpan.TotalHours;

                return hours;
            }
            return 0;
        }

        public int GetFeedbackCount(string id, bool isInfluencer)
        {
            if (isInfluencer)
            {
                return _feedbackRepo.GetAll().Where(x => x.InfluenterId == id).Count();
            }
            else
            {
                return _feedbackRepo.GetAll().Where(x => x.ApplicationUserId == id).Count();
            }
        }

        public double GetSingleScore(string id)
        {
            var feedback = _feedbackRepo.Get(id);

            var feedbackSum = 0.0;

            feedbackSum += feedback.Interaktion;
            feedbackSum += feedback.Opførsel;
            feedbackSum += feedback.Troværdighed;
            feedbackSum += feedback.Kvalitet;
            feedbackSum = feedbackSum / 4;

            return Math.Round(feedbackSum, 2);
        }

        public List<bool> GetStars(double value)
        {
            var round = Math.Round(value * 2, MidpointRounding.AwayFromZero) / 2;
            var floor = Math.Floor(round);
            var hasDeciaml = (round % 1) != 0;
            var list = new List<bool>();

            for (int i = 0; i <= floor; i++)
            {
                if (!hasDeciaml)
                {
                    if (i >= floor) return list;
                    list.Add(true);
                }
                else
                {
                    if (floor == i)
                        list.Add(false);
                    else
                        list.Add(true);
                }
            }
            return list;
        }

        public double GetTotalScore(string id)
        {
            var influencer = _influencerRepo.Get(id);

            var feedbacks = influencer.Ratings;

            if (feedbacks.Count == 0)
                return 0;

            int numberOfFeedbacks = 0;
            double allFeedbackSums = 0;

            foreach (var f in feedbacks)
            {
                double feedbackSum = 0;

                feedbackSum += f.Interaktion;
                feedbackSum += f.Opførsel;
                feedbackSum += f.Troværdighed;
                feedbackSum += f.Kvalitet;
                feedbackSum = feedbackSum / 4;

                // Antal ratings
                numberOfFeedbacks++;

                // Tilføjer dem til samlingen
                allFeedbackSums += feedbackSum;
            }

            double average = (allFeedbackSums / numberOfFeedbacks);

            return Math.Round(average, 2);
        }

        public int ReadFeedback(string id, string userId)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == userId);
            var influencer = _influencerRepo.Get(userId);
            var feedback = _feedbackRepo.Get(id);

            if (influencer == null)
            {
                feedback.IsAnswerRead = true;
                _feedbackRepo.Update(feedback);
                return _feedbackRepo.GetAll().Where(x => x.IsAnswerRead == false && x.Answer != null && x.ApplicationUserId == userId).Count();
            }
            else
            {
                feedback.IsRead = true;
                _feedbackRepo.Update(feedback);
                return influencer.Ratings.Where(x => x.IsRead == false).Count();
            }
        }

        public int UnreadFeedbackCount(string id)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == id);
            var influencer = _influencerRepo.Get(id);

            if (influencer != null)
                return influencer.Ratings.Where(x => x.IsRead == false).Count();
            else
                return _feedbackRepo.GetAll().Where(x => x.IsAnswerRead == false && x.Answer != null && x.ApplicationUserId == id).Count();
        }
    }
}
