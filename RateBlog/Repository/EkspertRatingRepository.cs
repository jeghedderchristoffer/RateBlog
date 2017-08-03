using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RateBlog.Models;
using RateBlog.Data;
using Microsoft.EntityFrameworkCore;

namespace RateBlog.Repository
{
    public class EkspertRatingRepository : IEkspertRatingRepository
    {

        private ApplicationDbContext _applicationDbContext;

        public EkspertRatingRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
            
        public void Add(EkspertRating ekspertrating)
        {
            _applicationDbContext.EkspertRating.Add(ekspertrating);
            _applicationDbContext.SaveChanges();
        }

  

        public void Delete(int Id)
        {
            EkspertRating ekspertrating = _applicationDbContext.EkspertRating.Find(Id);
            _applicationDbContext.EkspertRating.Remove(ekspertrating);
            _applicationDbContext.SaveChanges();
        }

        public EkspertRating Get(int Id)
        {
            return _applicationDbContext.EkspertRating.FirstOrDefault(x => x.Id == Id);
        }

        public List<EkspertRating> GetAll()
        {
            return _applicationDbContext.EkspertRating.ToList();
        }

        public double GetEkspertRatingAverage(int influenterId)
        {
            if (_applicationDbContext.EkspertRating.Any(x => x.InfluenterId == influenterId))
            {
                var ekspertratings = _applicationDbContext.EkspertRating.Where(x => x.InfluenterId == influenterId);
                int numberOfRatings = 0;
                double allRatingSums = 0;

                foreach (var v in ekspertratings)
                {
                    double ratingSum = 0;

                    // Tager alle værdier, plusser dem sammen og dividere dem med antallet af ratings == gennemsnit
                    var rating = Get(v.Id);
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

        public void Update(EkspertRating ekspertrating)
        {
            _applicationDbContext.Entry(ekspertrating).State = EntityState.Modified;
            _applicationDbContext.SaveChanges();
        }


        public List<EkspertRating> GetEkspertRatingForInfluenter(int influenterId)
        {
            return _applicationDbContext.EkspertRating.Where(x => x.InfluenterId == influenterId).ToList();
        }

        public double GetSingleEkspertRatingAverage(int Id)
        {
            var rating = Get(Id);
            double ratingSum = 0;

            // Tager alle værdier, plusser dem sammen og dividere dem med antallet af ratings == gennemsnit
            ratingSum += rating.Interaktion;
            ratingSum += rating.Opførsel;
            ratingSum += rating.Troværdighed;
            ratingSum += rating.Kvalitet;
            ratingSum = ratingSum / 4;

            return ratingSum * 20;
        }

        public int GetMyEkspertRatingNumber(string applicationUserId)
        {
            if (_applicationDbContext.EkspertRating.Any(x => x.ApplicationUserId == applicationUserId))
            {
                return _applicationDbContext.EkspertRating.Where(x => x.ApplicationUserId == applicationUserId).Count();
            }
            return 0;
        }

        public List<EkspertRating> GetAllForEkspert(string id)
        {
            return _applicationDbContext.EkspertRating.Where(x => x.ApplicationUserId == id).ToList();
        }

        public int CountEkspertRatings(int influenterId)
        {
            try
            {
                return _applicationDbContext.EkspertRating.Where(x => x.InfluenterId == influenterId).Count();
            }
            catch (ArgumentNullException)
            {
                return 0;
            }
        }

        public bool HasEkspertRatings(int influenterId)
        {
            if (_applicationDbContext.EkspertRating.Any(x => x.InfluenterId == influenterId))
            {
                return true;
            }
            return false;
        }


    }
}
