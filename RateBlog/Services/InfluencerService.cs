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
        private readonly IInfluencerRepository _influencerRepo;

        public InfluencerService(UserManager<ApplicationUser> userManger, IInfluencerRepository influencerRepo)
        {
            _influencerRepo = influencerRepo;
            _userManager = userManger; 
        }

        public List<ApplicationUser> GetInfluencers(IEnumerable<ApplicationUser> users)
        {
            var list = new List<ApplicationUser>(); 

            foreach(var v in users)
            {
                if(_influencerRepo.Get(v.Id) != null)
                {
                    list.Add(v); 
                }
            }
            return list; 
        }

        public bool IsInfluencerApproved(string id)
        {
            var influencer = _influencerRepo.Get(id); 

            if(influencer != null)
            {
                if (influencer.IsApproved)
                {
                    return true; 
                }
            }
            return false; 
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
