using Microsoft.AspNetCore.Identity;
using RateBlog.Models;
using RateBlog.Repository;
using RateBlog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Services
{
    public class InfluencerService : IInfluencerService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Influencer> _influencerRepo;

        public InfluencerService(UserManager<ApplicationUser> userManger, IRepository<Influencer> influencerRepo)
        {
            _influencerRepo = influencerRepo;
            _userManager = userManger; 
        }

        public async Task<bool> IsUserInfluencerAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var influencer = _influencerRepo.Get(id); 

            if (influencer != null)
            {
                return true; 
            }
            return false; 
        }
    }
}
