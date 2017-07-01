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
        private UserManager<ApplicationUser> _userManager;

        public InfluenterController(IInfluenterRepository influenter, UserManager<ApplicationUser> userManager)
        { 
            _influenter = influenter;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult Index(string search)
        {

            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }
            
            var influenter = _userManager.Users.Where(x => x.Name.ToLower().Contains(search.ToLower()) && x.InfluenterId.HasValue).ToList();

            var model = new IndexViewModel()
            {
                SearchString = search,
                InfluentList = influenter
                
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
