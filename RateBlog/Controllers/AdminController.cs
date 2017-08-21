using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RateBlog.Models;
using RateBlog.Models.AdminViewModels;
using RateBlog.Repository;
using RateBlog.Services;
using RateBlog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IInfluencerService _influencerService;
        private readonly IRepository<Influencer> _influencerRepo;
        private readonly IEmailSender _emailSender;

        public AdminController(IEmailSender emailSender, UserManager<ApplicationUser> userManager, IInfluencerService influencerService, IRepository<Influencer> influencerRepo)
        {
            _userManager = userManager;
            _influencerService = influencerService;
            _influencerRepo = influencerRepo;
            _emailSender = emailSender; 
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
        public async Task<IActionResult> ApproveInfluencer(string id)
        {
            var influencer = _influencerRepo.Get(id);
            var user = _userManager.Users.SingleOrDefault(x => x.Id == id); 
            influencer.IsApproved = true;
            _influencerRepo.Update(influencer);
            await _emailSender.SendInfluencerApprovedEmailAsync(user.Name, user.Email, influencer.Alias); 
            return RedirectToAction("UserProfile", new { id = id }); 
        }

        [HttpPost]
        public async Task<IActionResult> DisapproveInfluencer(string id)
        {
            var influencer = _influencerRepo.Get(id);
            var user = _userManager.Users.SingleOrDefault(x => x.Id == id);
            _influencerRepo.Delete(influencer);

            if (user.NormalizedEmail.StartsWith("USERINFLUENCER"))
            {
                await _userManager.DeleteAsync(user);
                return RedirectToAction("Index", "Admin"); 
            }

            await _emailSender.SendInfluencerDisapprovedEmailAsync(user.Name, user.Email); 
            return RedirectToAction("UserProfile", new { id = id });
        }

        [HttpGet]
        public IActionResult EditUser(string id)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == id);
            var influencer = _influencerRepo.Get(id);

            var model = new EditUserViewModel()
            {
                ApplicationUser = user,
                Influencer = influencer
            };

            return View(model); 
        }

        [HttpPost]
        public IActionResult EditUser(EditUserViewModel model)
        {



            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == id);
            var result = await _userManager.DeleteAsync(user);

            if(result.Succeeded)
                return RedirectToAction("Index", "Admin");
            else
                return RedirectToAction("UserProfile", "Admin", new { id = id });
        }

    }
}
