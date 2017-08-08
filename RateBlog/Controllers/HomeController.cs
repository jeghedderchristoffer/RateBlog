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
        private readonly IRepository<Category> _categoryRepo;
        private readonly IRepository<Platform> _platformRepo;
        private readonly IRepository<Influencer> _influencerRepo; 
        private readonly UserManager<ApplicationUser> _userManger;

        public HomeController(IRepository<Platform> platformRepo, IRepository<Category> categoryRepo, IRepository<Influencer> influencerRepo, UserManager<ApplicationUser> userManger)
        {
            _platformRepo = platformRepo;
            _categoryRepo = categoryRepo;
            _influencerRepo = influencerRepo;
            _userManger = userManger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult SearchHelp(string search)
        {
            var model = new SearchHelpModel();
            model.InfluencerList = new List<Influencer>();
            model.KategoriList = new List<Category>();
            model.PlatformList = new List<Platform>();

            if (!string.IsNullOrEmpty(search))
            {
                foreach (var kategori in _categoryRepo.GetAll())
                {
                    if (kategori.Name.ToLower().StartsWith(search.ToLower()))
                    {
                        model.KategoriList.Add(kategori); 
                    }
                }

                foreach (var platform in _platformRepo.GetAll())
                {
                    if (platform.Name.ToLower().StartsWith(search.ToLower()))
                    {
                        model.PlatformList.Add(platform); 
                    }
                }

                model.InfluencerList = _influencerRepo.GetAll().Where(x => x.Alias.ToLower().StartsWith(search.ToLower())).Take(5).ToList();
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
