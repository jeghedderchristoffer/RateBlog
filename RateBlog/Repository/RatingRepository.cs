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
                    ratingSum += rating.Aktivitet;
                    ratingSum += rating.Interaktion;
                    ratingSum += rating.SprogBrug;
                    ratingSum += rating.Troværdighed;
                    ratingSum += rating.Kvalitet;
                    ratingSum += rating.Orginalitet;
                    ratingSum = ratingSum / 6;

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


        // DET SKAL VÆRE one to many FORHOLD OG IKKE MANY TO MANY. 

        public List<Rating> GetRatingForInfluenter(int influenterId)
        {
            return null;
        }

        public int CountRatings(int influenterId)
        {
            try
            {
                return _applicationDbContext.Rating.Where(x => x.InfluenterId == influenterId).Count();
            }
            catch(ArgumentNullException)
            {
                return 0;
            }
        }
    }
}
