using RateBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Repository
{
    public interface IEkspertRatingRepository
    {
        EkspertRating Get(int Id);

        List<EkspertRating> GetAll();

        void Add(EkspertRating ekspertrating);

        void Update(EkspertRating ekspertrating);

        void Delete(int Id);

        double GetEkspertRatingAverage(int influenterId);

        List<EkspertRating> GetEkspertRatingForInfluenter(int influenterId);

        double GetSingleEkspertRatingAverage(int Id);

        int GetMyEkspertRatingNumber(string applicationUserId);

        List<EkspertRating> GetAllForEkspert(string id);

        int CountEkspertRatings(int influenterId);

        bool HasEkspertRatings(int influenterId);
    }
}
