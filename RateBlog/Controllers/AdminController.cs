using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
          //  var influenterlist = _influenter.GetAll();

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
            getUser.Name = vmodel.ApplicationUser.Name;

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



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFeedback(int Id)
        {
            _rating.Delete(Id);
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUser(int Id)
        {
            _admin.Delete(Id);
            return View();
        }

        //    public IActionResult SeMere(string searchString, string id)
        //    {
        //        users med ID
        //        var user = _influenter.GetByStringID(id);
        //        Uden id, det er dem her som skal skal sendes over, og mens detblvier sendt over skal de seperares
        //        var users = _userManager.Users;

        //        var model = new SeMereViewModel()
        //        {
        //           if (!String.IsNullOrEmpty(searchString))
        //        {
        //            model = _userManager.Users.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
        //        }
        //    };

        //        return View(model);


        //}
        //public IActionResult SeMere(string searchString, bool isInfluencer)
        //{
        //    var model = _userManager.Users;

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        model = _userManager.Users.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
        //    }
        //    else
        //    {
        //        model = _userManager.Users;
        //    }

        //    if (isInfluencer)
        //    {
        //        model = model.Where(x => x.InfluenterId.HasValue);
        //    }

        //    viewmodel.InfluentList = model.ToList();

        //    return View(viewmodel);

        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    var model = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
        //    _context.Users.Remove(model);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction("Admin", "Index");

        //}



    }
}

