using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RateBlog.Models.FooterViewModels;
using RateBlog.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RateBlog.Controllers
{
    public class FooterController : Controller
    {
        private readonly IEmailSender _mailSender;

        public FooterController(IEmailSender mailSender)
        {
            _mailSender = mailSender;
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

        public IActionResult FeedbackAndAnswers()
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

    }
}
