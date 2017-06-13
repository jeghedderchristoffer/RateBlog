using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RateBlog.Models;
using RateBlog.Data;
using Microsoft.EntityFrameworkCore;

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
    }
}
