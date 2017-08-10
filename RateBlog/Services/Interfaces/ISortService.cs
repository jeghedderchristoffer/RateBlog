using RateBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Services.Interfaces
{
    public interface ISortService
    {
        IEnumerable<ApplicationUser> InfluencerSortByPlatAndCat(int[] platformIds, int[] categoryIds, List<ApplicationUser> users);

        IEnumerable<ApplicationUser> SortInfluencer(int[] platforme, int[] kategorier, string search, int sortBy);
    }
}
