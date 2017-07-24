using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RateBlog.Models;
using RateBlog.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace RateBlog.Repository
{
    public class RatingRepository : IRatingRepository
    {
        private ApplicationDbContext _applicationDbContext;

        public RatingRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Add(Rating rating)
        {
            _applicationDbContext.Rating.Add(rating);
            _applicationDbContext.SaveChanges();
        }

        public void Delete(int RatingId)
        {
            Rating rating = _applicationDbContext.Rating.Find(RatingId);
            _applicationDbContext.Rating.Remove(rating);
            _applicationDbContext.SaveChanges();
        }

        public Rating Get(int Id)
        {
            return _applicationDbContext.Rating.FirstOrDefault(x => x.RatingId == Id);
        }

        public List<Rating> GetAll()
        {
            return _applicationDbContext.Rating.ToList();
        }

        public void Update(Rating rating)
        {
            _applicationDbContext.Entry(rating).State = EntityState.Modified;
            _applicationDbContext.SaveChanges();
        }

        public bool HasRatings(int influenterId)
        {
            if (_applicationDbContext.Rating.Any(x => x.InfluenterId == influenterId))
            {
                return true;
            }
            return false;
        }

        public double GetRatingAverage(int influenterId)
        {
            if (_applicationDbContext.Rating.Any(x => x.InfluenterId == influenterId))
            {
                var ratings = _applicationDbContext.Rating.Where(x => x.InfluenterId == influenterId);
                int numberOfRatings = 0;
                double allRatingSums = 0;

                foreach (var v in ratings)
                {
                    double ratingSum = 0;

                    // Tager alle værdier, plusser dem sammen og dividere dem med antallet af ratings == gennemsnit
                    var rating = Get(v.RatingId);
                    ratingSum += rating.Interaktion;
                    ratingSum += rating.Opførsel;
                    ratingSum += rating.Troværdighed;
                    ratingSum += rating.Kvalitet;
                    ratingSum = ratingSum / 4;

                    // Antal ratings
                    numberOfRatings++;

                    // Tilføjer dem til samlingen
                    allRatingSums += ratingSum;
                }

                double average = (allRatingSums / numberOfRatings) * 20;

                return average;
            }

            return 0;
        }

        public int GetUnreadRatingsNumber(int influenterId)
        {
            var ratings = _applicationDbContext.Rating.Where(x => x.InfluenterId == influenterId);

            int number = 0;

            foreach (var v in ratings)
            {
                if (Get(v.RatingId).IsRead == false)
                {
                    number++;
                }
            }

            return number;
        }

        public List<Rating> GetRatingForInfluenter(int influenterId)
        {
            return _applicationDbContext.Rating.Where(x => x.InfluenterId == influenterId).ToList();
        }

        public int CountRatings(int influenterId)
        {
            try
            {
                return _applicationDbContext.Rating.Where(x => x.InfluenterId == influenterId).Count();
            }
            catch (ArgumentNullException)
            {
                return 0;
            }
        }

        public double GetSingleRatingAverage(int ratingId)
        {
            var rating = Get(ratingId);
            double ratingSum = 0;

            // Tager alle værdier, plusser dem sammen og dividere dem med antallet af ratings == gennemsnit
            ratingSum += rating.Interaktion;
            ratingSum += rating.Opførsel;
            ratingSum += rating.Troværdighed;
            ratingSum += rating.Kvalitet;
            ratingSum = ratingSum / 4;

            return ratingSum * 20;
        }

        public int GetSingleRating(int ratingId, string name)
        {
            return 0;
        }

        public int GetInfluenterRatingNumber(int influenterId)
        {
            if (_applicationDbContext.Rating.Any(x => x.InfluenterId == influenterId))
            {
                return _applicationDbContext.Rating.Where(x => x.InfluenterId == influenterId).Count();
            }
            return 0;
        }

        public int GetMyRatingNumber(string applicationUserId)
        {
            if (_applicationDbContext.Rating.Any(x => x.ApplicationUserId == applicationUserId))
            {
                return _applicationDbContext.Rating.Where(x => x.ApplicationUserId == applicationUserId).Count();
            }
            return 0;
        }

        public int GetMyRatingAnswerNumber(string applicationUserId)
        {
            if (_applicationDbContext.Rating.Any(x => x.ApplicationUserId == applicationUserId && x.Answer != null))
            {
                return _applicationDbContext.Rating.Where(x => x.ApplicationUserId == applicationUserId && x.Answer != null).Count();
            }
            return 0;
        }

        public int GetInfluenterAnswerNumber(int influenterId)
        {
            if (_applicationDbContext.Rating.Any(x => x.InfluenterId == influenterId && x.Answer != null))
            {
                return _applicationDbContext.Rating.Where(x => x.InfluenterId == influenterId && x.Answer != null).Count();
            }
            return 0;
        }

        public double GetAnswerPercentageForInfluencer(int influenterId)
        {
            if (_applicationDbContext.Rating.Any(x => x.InfluenterId == influenterId && x.Answer != null))
            {
                var numberOfAnswer = _applicationDbContext.Rating.Where(x => x.InfluenterId == influenterId && x.Answer != null).Count();
                var numberOfRatings = _applicationDbContext.Rating.Where(x => x.InfluenterId == influenterId && x.IsRead == true).Count();

                double result = (100.0 / numberOfRatings) * numberOfAnswer;

                return result;

            }
            return 0;
        }

        public double GetHoursLeftToRate(string applicationUserId, int influenterId)
        {
            if (_applicationDbContext.Rating.Any(x => x.InfluenterId == influenterId && x.ApplicationUserId == applicationUserId))
            {
                var user = _applicationDbContext.Users.SingleOrDefault(x => x.Id == applicationUserId);
                var rating = _applicationDbContext.Rating.Where(x => x.InfluenterId == influenterId && x.ApplicationUserId == applicationUserId).OrderByDescending(x => x.RateDateTime).FirstOrDefault();

                var timeSpan = DateTime.Now - rating.RateDateTime;
                var hours = timeSpan.TotalHours;

                return hours;
            }

            return 0;


            
        }
    }
}
