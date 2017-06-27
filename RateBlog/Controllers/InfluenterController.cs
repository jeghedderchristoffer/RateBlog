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







    }
}
