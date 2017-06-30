using RateBlog.Data;
using RateBlog.Models;
using RateBlog.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Repository
{
    public class InfluenterRatingRepository : IInfluenterRatingRepository
    {
        private ApplicationDbContext _dbContext;

        public InfluenterRatingRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public void Add(int influenterId, int ratingId)
        {
            _dbContext.InfluenterRating.Add(new InfluenterRating()
            {
                InfluenterId = influenterId,
                RatingId = ratingId
            });
            _dbContext.SaveChanges(); 
        }
    }
}
