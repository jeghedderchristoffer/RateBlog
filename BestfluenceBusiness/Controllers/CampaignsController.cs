using BestfluenceBusiness.Models.CampaignsViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BestfluenceBusiness.Controllers
{
    public class CampaignsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/[controller]/create/[action]")]
        public IActionResult Step1()
        {
            return View();
        }

        [HttpPost]
        [Route("/[controller]/create/[action]")]
        public IActionResult Step2(CreateViewModel model)
        {
            if (TryValidateModel(model.Step1))
            {
                return View(model);
            }

            return RedirectToAction("Step1");
        }
    }
}
