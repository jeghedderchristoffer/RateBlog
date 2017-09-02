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
using RateBlog.Services;
using RateBlog.Models.FooterViewModels;

namespace RateBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Category> _categoryRepo;
        private readonly IRepository<Platform> _platformRepo;
        private readonly IInfluencerRepository _influencerRepo; 
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _mailSender;

        public HomeController(IEmailSender mailSender, SignInManager<ApplicationUser> signInManager, IRepository<Platform> platformRepo, IRepository<Category> categoryRepo, IInfluencerRepository influencerRepo, UserManager<ApplicationUser> userManger)
        {
            _platformRepo = platformRepo;
            _categoryRepo = categoryRepo;
            _influencerRepo = influencerRepo;
            _userManger = userManger;
            _signInManager = signInManager;
            _mailSender = mailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Presse()
        {
            return View();
        }

        public IActionResult Job()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _mailSender.SendEmailAsync(model.Name, "support@bestfluence.com", model.Title, "Fra email: " + model.Email + "<br><br>" + model.Text);
                TempData["Success"] = "Du har sendt en mail. Vi kontakter dig hurtigst muligt!";
                return View();
            }

            return View(model);
        }

        public IActionResult How()
        {
            return View();
        }

        public IActionResult Parents()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult Kommunikation()
        {
            return View();
        }

        public IActionResult UserGuidelines()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult TermsAndConditions()
        {
            return View();
        }

        public PartialViewResult LoginPartial()
        {
            return PartialView("_LoginPartial");
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

        public IActionResult Error()
        {
            return View();
        }
    }
}
