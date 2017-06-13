using RateBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Repository
{
    interface IRatingRepository
    {
        
        Rating Get(int Id);

        List<Rating> GetAll();

        void Add(Rating rating);

        void Update(Rating rating);

        void Delete(int RatingId);
    }
}
