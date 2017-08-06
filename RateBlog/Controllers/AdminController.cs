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
        // private IAdminRepository _admin;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IInfluenterRepository influenter)
        {
            _influenter = influenter;
            _context = context;
            _userManager = userManager;
            // _admin = admin;
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

