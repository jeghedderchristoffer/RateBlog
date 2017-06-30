using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Repository.Interface
{
    public interface IInfluenterRatingRepository
    {
        void Add(int influenterId, int ratingId);
    }
}
