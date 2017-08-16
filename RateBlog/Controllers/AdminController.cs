using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RateBlog.Models;
using RateBlog.Models.AdminViewModels;
using RateBlog.Repository;
using RateBlog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IInfluencerService _influencerService;
        private readonly IRepository<Influencer> _influencerRepo; 

        public AdminController(UserManager<ApplicationUser> userManager, IInfluencerService influencerService, IRepository<Influencer> influencerRepo)
        {
            _userManager = userManager;
            _influencerService = influencerService;
            _influencerRepo = influencerRepo; 
        }

        [HttpGet]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }

            var listOfUsers = _userManager.Users.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();

            var notApprovedList = new List<ApplicationUser>(); 

            foreach(var v in _influencerService.GetInfluencers(listOfUsers))
            {
                if (!_influencerService.IsInfluencerApproved(v.Id))
                {
                    notApprovedList.Add(v); 
                }
            }

            var model = new IndexViewModel()
            {
                AllUsers = listOfUsers,
                InfluencerApprovedList = notApprovedList
            };

            return View(model); 
        }

        [HttpGet]
        public IActionResult UserProfile(string id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);            
            return View(user);
        }

        [HttpPost]
        public IActionResult ApproveInfluencer(string id)
        {
            var influencer = _influencerRepo.Get(id);
            influencer.IsApproved = true;
            _influencerRepo.Update(influencer); 
            return RedirectToAction("UserProfile", new { id = id }); 
        }

    }
}
