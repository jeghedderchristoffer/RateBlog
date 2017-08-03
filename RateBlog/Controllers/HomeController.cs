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
using RateBlog.Helper;

namespace RateBlog.Controllers
{
    public class HomeController : Controller
    {
        private IKategoriRepository _kategoriRepo;
        private IPlatformRepository _platformRepo;
        private IInfluenterRepository _influenterRepo;
        private UserManager<ApplicationUser> _userManger;

        public HomeController(IPlatformRepository platformRepo, IKategoriRepository kategoriRepo, IInfluenterRepository influenterRepo, UserManager<ApplicationUser> userManger)
        {
            _platformRepo = platformRepo;
            _kategoriRepo = kategoriRepo;
            _influenterRepo = influenterRepo;
            _userManger = userManger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult SearchHelp(string search)
        {
            var model = new SearchHelpModel();
            model.InfluencerList = new List<Influenter>();
            model.KategoriList = new List<Kategori>();
            model.PlatformList = new List<Platform>();

            if (!string.IsNullOrEmpty(search))
            {
                foreach (var kategori in _kategoriRepo.GetAll())
                {
                    if (kategori.KategoriNavn.ToLower().StartsWith(search.ToLower()))
                    {
                        model.KategoriList.Add(kategori); 
                    }
                }

                foreach (var platform in _platformRepo.GetAll())
                {
                    if (platform.PlatformNavn.ToLower().StartsWith(search.ToLower()))
                    {
                        model.PlatformList.Add(platform); 
                    }
                }

                model.InfluencerList = _influenterRepo.GetAll().Where(x => x.Alias.ToLower().StartsWith(search.ToLower())).Take(5).ToList();
            }

            return PartialView("_SearchHelpPartial", model);
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
