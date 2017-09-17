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
using RateBlog.Data;

namespace RateBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailSender _mailSender;
        private readonly ApplicationDbContext _context;

        public HomeController(IEmailSender mailSender, ApplicationDbContext context)
        {
            _mailSender = mailSender;
            _context = context;
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

        public IActionResult FeedbackAndAnswers()
        {
            return View();
        }

        public PartialViewResult LoginPartial()
        {
            return PartialView("_LoginPartial");
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Test()
        {
            return View(); 
        }
    }
}
