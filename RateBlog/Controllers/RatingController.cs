using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RateBlog.Models;
using RateBlog.Models.RatingViewModels;
using RateBlog.Repository;
using RateBlog.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Controllers
{
    public class RatingController : Controller
    {

        private IRatingRepository _rating;
        private IInfluenterRepository _influenterRepo;
        private IInfluenterRatingRepository _influenterRatingRepo; 
        private UserManager<ApplicationUser> _userManger;

        public RatingController(IRatingRepository rating, IInfluenterRepository influenterRepo, IInfluenterRatingRepository influenterRatingRepo, UserManager<ApplicationUser> userManger)
        {
            _rating = rating;
            _influenterRepo = influenterRepo;
            _influenterRatingRepo = influenterRatingRepo;
            _userManger = userManger;
        }

        public IActionResult Index(int id) 
        {
            var influenter = _influenterRepo.Get(id);
            var model = new RatingViewModel()
            {
                Influenter = influenter
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RateInfluenter(int orginalitet, int kvalitet, int troværdighed, int interaktion, int aktivitet, int antalÅr, RatingViewModel model)
        {
            var user = await _userManger.GetUserAsync(User); 

            // Værdier til antal tid fulgt: 01 == 0-1 år, 12 == 1-2 år, 2 == 2+ år!! 
            var rating = new Rating()
            {
                Orginalitet = orginalitet, 
                Kvalitet = kvalitet, 
                Troværdighed = troværdighed, 
                Interaktion = interaktion, 
                Aktivitet = aktivitet, 
                TidFulgt = antalÅr, 
                Review = model.Review, 
                ApplicationUserId = user.Id
            };

            // Tilføjer til Rating tabellen
            _rating.Add(rating);

            // Tilføjer til Join Tabellen InfluenterRating
            _influenterRatingRepo.Add(model.Influenter.InfluenterId, rating.RatingId);

            // Der mangler at tjekke om denne user allerede har rated denne influenter....!!!!!!

            // Lidt feedback til brugeren
            TempData["Success"] = "Du har givet dit feedback til " + model.Influenter.Alias;

            // Skal ændres til Influenter Controller, ShowInfluenter Action
            return RedirectToAction("Index", "Influenter", new { });
        }

    }

}

