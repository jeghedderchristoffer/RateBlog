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

        public IActionResult Index(int influenterId)
        {
            var influenter = _influenterRepo.Get(influenterId);
            var model = new RatingViewModel()
            {
                Influenter = influenter
            };

            return View(model);
        }

        public async Task<JsonResult> RateInfluenter(int orginalitet, int kvalitet, int troværdighed, int interaktion, int aktivitet, int antalÅr, RatingViewModel model)
        {
            var user = await _userManger.GetUserAsync(User); 

            var orginalitetRating = orginalitet;
            var kvalitetRating = kvalitet;
            var troværdighedRating = troværdighed;
            var interaktionRating = interaktion;
            var aktivitetRating = aktivitet;

            // Værdier: 01 == 0-1 år, 12 == 1-2 år, 2 == 2+ år!! 
            var antalÅrFulgt = antalÅr;

            var review = model.Review;







            return null;
        }

    }

}

