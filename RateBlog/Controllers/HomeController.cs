using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using RateBlog.Repository;
using Microsoft.AspNetCore.Identity;
using RateBlog.Models;

namespace RateBlog.Controllers
{
    public class HomeController : Controller
    {
        private IKategoriRepository _kategoriRepo;
        private IInfluenterRepository _influenterRepo;
        private UserManager<ApplicationUser> _userManger;

        public HomeController(IKategoriRepository kategoriRepo, IInfluenterRepository influenterRepo, UserManager<ApplicationUser> userManger)
        {
            _kategoriRepo = kategoriRepo;
            _influenterRepo = influenterRepo;
            _userManger = userManger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult Autocomplete(string prefix)
        {
            var strings = _kategoriRepo.GetAll().Where(x => x.KategoriNavn.StartsWith(prefix));



            //var st = from s in _kategoriRepo.GetAll()
            //         where s.KategoriNavn.StartsWith(prefix)
            //         select s.KategoriNavn; 

            return Json(strings); 
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
