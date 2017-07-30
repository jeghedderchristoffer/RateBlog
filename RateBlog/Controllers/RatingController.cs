using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RateBlog.Models;
using RateBlog.Models.RatingViewModels;
using RateBlog.Repository;
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
        private UserManager<ApplicationUser> _userManger;

        public RatingController(IRatingRepository rating, IInfluenterRepository influenterRepo, UserManager<ApplicationUser> userManger)
        {
            _rating = rating;
            _influenterRepo = influenterRepo;
            _userManger = userManger;
        }

        [HttpGet]
        [Authorize]
        public IActionResult RateInfluencer(int id)
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
        public async Task<IActionResult> RateInfluencer(int kvalitet, int troværdighed, int opførsel, int interaktion, bool? anbefaling, RatingViewModel model)
        {
            var user = await _userManger.GetUserAsync(User);

            if (user.InfluenterId.HasValue)
            {
                if (model.Influenter.InfluenterId == user.InfluenterId.Value)
                {
                    TempData["Error"] = "Du kan ikke anmelde dig selv!";
                    return RedirectToAction("RateInfluencer");
                }
                else
                {
                    TempData["Error"] = "Influencers kan ikke anmelde andre influencers";
                    return RedirectToAction("RateInfluencer");
                }
            }

            var hoursSinceLastRating = _rating.GetHoursLeftToRate(user.Id, model.Influenter.InfluenterId);

            // Så har denne bruger ikke ratet denne influencer endnu
            if (hoursSinceLastRating == 0)
            {
                // Gør ingenting?? :-)
            }
            // Så har denne bruger ratet indenfor 24 timer...
            else if (hoursSinceLastRating < 24)
            {
                var hours = TimeSpan.FromHours(24 - hoursSinceLastRating);
                TempData["Error"] = "Du kan anmelde denne influencer igen om " + hours.ToString(@"hh\:mm") + " minutter";
                return RedirectToAction("RateInfluencer");
            }

            if (opførsel == 0 || kvalitet == 0 || troværdighed == 0 || interaktion == 0 || model.Review == null || anbefaling == null)
            {
                TempData["Error"] = "Du skal udfylde alle felterne for at give dit feedback";

                var errorModel = new RatingViewModel()
                {
                    Review = model.Review,
                    Influenter = model.Influenter
                }; 

                return View("RateInfluencer", errorModel);
            }            

            var rating = new Rating()
            { 
                Kvalitet = kvalitet, 
                Troværdighed = troværdighed, 
                Opførsel = opførsel,
                Interaktion = interaktion, 
                Feedback = model.Review, 
                Anbefaling = anbefaling,
                ApplicationUserId = user.Id, 
                InfluenterId = model.Influenter.InfluenterId, 
                RateDateTime = DateTime.Now
            };

            // Tilføjer til Rating tabellen
            _rating.Add(rating);

            // Der mangler at tjekke om denne user allerede har rated denne influenter....!!!!!!

            // Lidt feedback til brugeren
            TempData["Success"] = "Du har givet din anmeldelse " + model.Influenter.Alias;

            // Skal ændres til Influenter Controller, ShowInfluenter Action
            return RedirectToAction("Show", "Influencer", new { Id = model.Influenter.InfluenterId });
        }

        public JsonResult GetAverageRating(int influenterId)
        {
            var averageRating = _rating.GetRatingAverage(influenterId);

            return Json(averageRating);
        }



    }

}

