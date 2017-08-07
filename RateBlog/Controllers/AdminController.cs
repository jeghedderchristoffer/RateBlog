using System;
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


        private readonly ApplicationDbContext _context;
        private IInfluenterRepository _influenter;
        private readonly UserManager<ApplicationUser> _userManager;
        private IAdminRepository _admin;
        private IRatingRepository _rating;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IInfluenterRepository influenter, IAdminRepository admin, IRatingRepository rating)
        {
            _rating = rating;
            _influenter = influenter;
            _context = context;
            _userManager = userManager;
            _admin = admin;
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
        

            var user = _influenter.GetByStringID(id);

            var model = new SeMereViewModel()
            {
                ApplicationUser = user,

               
             };

            return View(model);

        }

        public IActionResult EditUser(string id)
        {
            
            var user = _influenter.GetByStringID(id);
           
            var model = new SeMereViewModel()
            {
                ApplicationUser = user,
            };

            return View(model);

        }
        [HttpPost]
        public IActionResult EditUser(SeMereViewModel vmodel)
        {
            var getUser = _influenter.GetByStringID(vmodel.ApplicationUser.Id);
            getUser.Email = vmodel.ApplicationUser.Email;
            getUser.Name = vmodel.ApplicationUser.Name;

            getUser.City = vmodel.ApplicationUser.City;
            getUser.InfluenterId = vmodel.ApplicationUser.InfluenterId;
            getUser.ProfileText = vmodel.ApplicationUser.ProfileText;
            getUser.PhoneNumber = vmodel.ApplicationUser.PhoneNumber;
            getUser.PasswordHash = vmodel.ApplicationUser.PasswordHash;
            getUser.LockoutEnd = vmodel.ApplicationUser.LockoutEnd;


            _admin.EditUser(getUser);

            var model = new SeMereViewModel()
            {
                ApplicationUser = getUser,

            };

            return View(model);

        }

        [HttpGet]
        public IActionResult SeFeedback(int Id)
        {
            var getAllRatings = _rating.GetRatingForInfluenter(Id);

            var rating = new SeFeedbackViewModel()
            {
                ListRating = getAllRatings
            };

            return View(rating);
        }

        [HttpGet]
        public IActionResult RedigereFeedback(int Id)
        {
            var getRating = _rating.Get(Id);

            var getUserName = _userManager.Users.SingleOrDefault(x => x.Id == getRating.ApplicationUserId).Name;



            var rating = new SeFeedbackViewModel()
            {
                Rating = getRating,
                AnmelderNavn = getUserName

            };

            return View(rating);
        }


        //update rateing
        [HttpPost]
        public IActionResult RedigereFeedback(SeFeedbackViewModel SeFeedBackModel)
        {
            var getRating = _rating.Get(SeFeedBackModel.Rating.RatingId);
            getRating.Kvalitet = SeFeedBackModel.Rating.Kvalitet;
            getRating.Opførsel = SeFeedBackModel.Rating.Opførsel;
            getRating.Interaktion = SeFeedBackModel.Rating.Interaktion;
            getRating.Troværdighed = SeFeedBackModel.Rating.Troværdighed;
            getRating.Feedback = SeFeedBackModel.Rating.Feedback;
            getRating.Answer = SeFeedBackModel.Rating.Answer;
            _rating.Update(getRating);
            var getrating = _rating.Get(SeFeedBackModel.Rating.RatingId);

            var rating = new SeFeedbackViewModel()
            {
                Rating = getrating
            };

            return View(rating);
        }



       [HttpPost]
        public IActionResult DeleteFeedback(int Id)
        {
            _rating.Delete(Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteUser(string Id)
        {
            _admin.Delete(Id);
            return RedirectToAction("Index");
        }

        public IActionResult BanUser10(int id)
        {
            var user = _userManager.Users.First();
            user.LockoutEnd = DateTime.Now.AddDays(10);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult BanUser25(int id)
        {
            var user = _userManager.Users.First();
            user.LockoutEnd = DateTime.Now.AddDays(25);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult BanUser100(int id)
        {
            var user = _userManager.Users.First();
            user.LockoutEnd = DateTime.Now.AddDays(25);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }





    }
}

