using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RateBlog.Data;
using RateBlog.Models;
using RateBlog.Models.InfluenterViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RateBlog.Controllers
{
    public class AdminController : Controller
    {
        // GET: /<controller>/
      
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController (ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IndexViewModel viewmodel = new IndexViewModel();

        //public IActionResult Index()
        //{
        //    
        //   

        

        //    return View(viewmodel);
        //}


        public IActionResult Index(string searchString)
        {
            var model = _userManager.Users.ToList();
            viewmodel.InfluentList = model.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                _context.Users.Where(s => s.Name.Contains(searchString));
            }

            viewmodel.SearchString = searchString;

            return View(viewmodel);
        }

        public IActionResult ShowUser(string searchString)
        {
            var model = _userManager.Users.ToList();
            viewmodel.InfluentList = model.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                _context.Users.Where(s => s.Name.Contains(searchString));
            }

            viewmodel.SearchString = searchString;
            return View(viewmodel);
        }

    }
}
