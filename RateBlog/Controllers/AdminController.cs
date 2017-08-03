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
using RateBlog.Models.InfluenterViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RateBlog.Controllers
{
    public class AdminController : Controller
    {
        // GET: /<controller>/

        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var model = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            _context.Users.Remove(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Admin", "Index");

        }
        //[http]
        //public async Task<IActionResult> Edit()
        //{

        //}
        public IndexViewModel viewmodel = new IndexViewModel();

        //public IActionResult Index()
        //{

        //    return View(viewmodel);
        //}


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

            //if (SortByRole != 0)
            //{
            //    model = model.Where(s => !s.InfluenterId.Equals(0));
            //}

            if (isInfluencer)
            {
                model = model.Where(x => x.InfluenterId.HasValue);
            }

            viewmodel.InfluentList = model.ToList();

            return View(viewmodel);
        }

        public IActionResult seMere()
        {
            return View();
        }

    }

   

    //public IActionResult ShowUser(string searchString, bool isInfluencer)
    //    {
    //        var model = _userManager.Users;

    //        if (!String.IsNullOrEmpty(searchString))
    //        {
    //            model = _userManager.Users.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
    //        }
    //        else
    //        {
    //            model = _userManager.Users;
    //        }

    //        //if (SortByRole != 0)
    //        //{
    //        //    model = model.Where(s => !s.InfluenterId.Equals(0));
    //        //}

    //        if (isInfluencer)
    //        {
    //            model = model.Where(x => x.InfluenterId.HasValue);
    //        }

    //        viewmodel.InfluentList = model.ToList();

    //        return View(viewmodel);
    //    }

    //public IActionResult ShowUser(string searchString)
    //{
    //    var model = _userManager.Users;

    //    if (!String.IsNullOrEmpty(searchString))
    //    {
    //        model = _userManager.Users.Where(s => s.Name.Contains(searchString));
    //    }
    //    else
    //    {
    //        model = _userManager.Users;
    //    }


    //    viewmodel.InfluentList = model.ToList();

    //    return View(viewmodel);
    //}

//    }
}
