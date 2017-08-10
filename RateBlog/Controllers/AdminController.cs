﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RateBlog.Data;
using RateBlog.Models;
using RateBlog.Models.AdminViewModels;
using RateBlog.Repository;
//using RateBlog.Models.InfluenterViewModels;

namespace RateBlog.Controllers
{

    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IRepository<Influencer> _influenter;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Feedback> _feedBack;

        public AdminController(UserManager<ApplicationUser> userManager, IRepository<Influencer> influenter, IRepository<Feedback> feedBack)
        {
            _feedBack = feedBack;
            _influenter = influenter;
      
            _userManager = userManager;
      
        }


        public IndexViewModel viewmodel = new IndexViewModel();

        public IActionResult Index(string searchString, bool isInfluencer)
        {
            var model = _userManager.Users;

            if (!String.IsNullOrEmpty(searchString))
            {
                model = _userManager.Users.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
            }
            else
            {
                model = _userManager.Users;
            }

            if (isInfluencer)
            {
                model = model.Where(x => x.InfluenterId.HasValue);
            }

            viewmodel.InfluentList = model.ToList();

            return View(viewmodel);
        }

        [HttpGet]
        public IActionResult SeMere(string id)
        {


            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);

            var model = new SeMereViewModel()
            {
                ApplicationUser = user,

               
             };

            return View(model);

        }

        public IActionResult EditUser(string id)
        {

            var user = _userManager.Users.FirstOrDefault(x => x.Id == id); 
           
            var model = new SeMereViewModel()
            {
                ApplicationUser = user,
            };

            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> EditUser(SeMereViewModel vmodel)
        {
            var getUser = _userManager.Users.SingleOrDefault(x => x.Id == vmodel.ApplicationUser.Id);
            getUser.Email = vmodel.ApplicationUser.Email;
            getUser.Name = vmodel.ApplicationUser.Name;
           

            getUser.InfluenterId = vmodel.ApplicationUser.InfluenterId;
            getUser.ProfileText = vmodel.ApplicationUser.ProfileText;
            getUser.PhoneNumber = vmodel.ApplicationUser.PhoneNumber;
            getUser.PasswordHash = vmodel.ApplicationUser.PasswordHash;
            getUser.LockoutEnd = vmodel.ApplicationUser.LockoutEnd;


            // _userManager.EditUser(getUser);
            var result = await _userManager.UpdateAsync(getUser);
         

            var model = new SeMereViewModel()
            {
                ApplicationUser = getUser,

            };

            return View(model);

        }

        [HttpGet]
        public IActionResult SeFeedback()
        {
            //var getAllRatings = _rating.GetRatingForInfluenter(Id);
            var getAllRatings = _feedBack.GetAll();
            var rating = new SeFeedbackViewModel()
            {
                ListRating = getAllRatings.ToList()
            };

            return View(rating);
        }

        [HttpGet]
        public IActionResult RedigereFeedback(int Id)
        {
            var getRating = _feedBack.Get(Id);

            var getUserName = _userManager.Users.SingleOrDefault(x => x.Id == getRating.ApplicationUserId).Name;



            var rating = new SeFeedbackViewModel()
            {
                feedBack = getRating,
                AnmelderNavn = getUserName

            };

            return View(rating);
        }


        //update rateing
        [HttpPost]
        public IActionResult RedigereFeedback(SeFeedbackViewModel SeFeedBackModel)
        {
            var getRating = _feedBack.Get(SeFeedBackModel.feedBack.Id);
            getRating.Kvalitet = SeFeedBackModel.feedBack.Kvalitet;
            getRating.Opførsel = SeFeedBackModel.feedBack.Opførsel;
            getRating.Interaktion = SeFeedBackModel.feedBack.Interaktion;
            getRating.Troværdighed = SeFeedBackModel.feedBack.Troværdighed;
           // getRating.Feedback = SeFeedBackModel.Rating.Feedback;
            getRating.Answer = SeFeedBackModel.feedBack.Answer;
            _feedBack.Update(getRating);
            var getrating = _feedBack.Get(SeFeedBackModel.feedBack.Id);

            var rating = new SeFeedbackViewModel()
            {
               feedBack = getrating
            };

            return View(rating);
        }



       [HttpPost]
        public IActionResult DeleteFeedback(int Id)
        {
             var getfeedback = _feedBack.Get(Id);
            _feedBack.Delete(getfeedback);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteUser(string id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }

        //public IActionResult BanUser10(int id)
        //{
        //    var user = _userManager.Users.First();
        //    user.LockoutEnd = DateTime.Now.AddDays(10);
            
        //    _context.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        public async Task<IActionResult> BanUser(string id, int dage)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            user.LockoutEnd = DateTime.Now.AddDays(dage);
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["ErrorMessage"] = "Der skete en fejl"; 
                return View(); 
            }

            

        }

        //public IActionResult BanUser25(string id)
        //{
        //    var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
        //    user.LockoutEnd = DateTime.Now.AddDays(25);
        //    _context.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //public IActionResult BanUser100(int id)
        //{
        //    var user = _userManager.Users.First();
        //    user.LockoutEnd = DateTime.Now.AddDays(25);
        //    _context.SaveChanges();
        //    return RedirectToAction("Index");
        //}





    }
}

