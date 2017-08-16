using RateBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Services.Interfaces
{
    public interface IInfluencerService
    {
        Task<bool> IsUserInfluencerAsync(string id);

        bool IsInfluencerApproved(string id);

        List<ApplicationUser> GetInfluencers(IEnumerable<ApplicationUser> users);
    }
}
