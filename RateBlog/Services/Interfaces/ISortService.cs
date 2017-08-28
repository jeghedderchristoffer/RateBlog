using RateBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Services.Interfaces
{
    public interface ISortService
    {

        IEnumerable<Influencer> SortInfluencer(string[] platforme, string[] kategorier, string search, int sortBy);
    }
}
