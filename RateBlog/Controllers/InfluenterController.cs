using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RateBlog.Data;
using RateBlog.Models;
using RateBlog.Models.InfluenterViewModels;
using RateBlog.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Controllers
{
    public class InfluenterController : Controller
    {

        private IInfluenterRepository _influenter;
        private IRatingRepository _ratingRepository;
        private UserManager<ApplicationUser> _userManager;

        public InfluenterController(IInfluenterRepository influenter, IRatingRepository ratingRepository, UserManager<ApplicationUser> userManager)
        { 
            _influenter = influenter;
            _userManager = userManager;
            _ratingRepository = ratingRepository;
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            Dictionary<int, double> influenterRating = new Dictionary<int, double>(); 

            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }
            
            var influenter = _userManager.Users.Where(x => x.Name.ToLower().Contains(search.ToLower()) && x.InfluenterId.HasValue).ToList();

            foreach(var v in influenter)
            {
                influenterRating.Add(v.InfluenterId.Value, _ratingRepository.GetRatingAverage(v.InfluenterId.Value)); 
            }

            var model = new IndexViewModel()
            {
                SearchString = search,
                InfluentList = influenter, 
                InfluenterRatings = influenterRating             
            };

            return View(model); 
        }

        [HttpGet]
        public IActionResult Show(int id)
        {
            var influenter = _influenter.Get(id);

            //Burde kun kunne få den pågældene user, da Index() metoden KUN returnere Users som er influenter...
            var user = _userManager.Users.SingleOrDefault(x => x.InfluenterId == id);

            var model = new ShowViewModel()
            {
                ApplicationUser = user,
                Influenter = influenter
            };

            return View(model);
        }







    }
}
