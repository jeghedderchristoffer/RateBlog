using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RateBlog.Repository;
using RateBlog.Models;
using Microsoft.AspNetCore.Identity;
using RateBlog.Models.EkspertViewModels;
using Microsoft.AspNetCore.Authorization;

namespace RateBlog.Controllers
{
    [Authorize(Roles = "Ekspert")]
    public class EkspertController : Controller
    {

        private readonly IRepository<Influencer> _influencerRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<ExpertFeedback> _expertFeedbackRepo;

        public EkspertController( IRepository<Influencer> influencer, IRepository<ExpertFeedback> expertFeedback, UserManager<ApplicationUser> userManager)
        {
            _influencerRepo = influencer;
            _userManager = userManager;
            _expertFeedbackRepo = expertFeedback;
            
        }
   
        public IActionResult Index(string search)
        {
            Dictionary<int, double> influenterRating = new Dictionary<int, double>();

            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }

            var influenter = _userManager.Users.
                Where(x => (x.Name.ToLower().Contains(search.ToLower())) && x.InfluenterId.HasValue
                || (x.Influenter.Alias.Contains(search) && x.InfluenterId.HasValue)).ToList();


            var model = new IndexViewModel()
            {
                SearchString = search,
                InfluentList = influenter,

            };
            return View(model);
        }

        [HttpGet]
        public IActionResult EkspertRating(int id)
        {
            var influenter = _influencerRepo.Get(id);
            var model = new EkspertRatingViewModel()
            {
                Influenter = influenter
            };

            return View(model);
        }


        [HttpGet]
        public IActionResult Anmeldelser(int id)
        {
            var influenter = _influencerRepo.Get(id);
            var model = new AnmeldelseViewModel()
            {
                Influenter = influenter
            };

            return View(model);
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EkspertRating(EkspertRatingViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);


            if (ModelState.IsValid)
            {
                var ekspertrating = new ExpertFeedback()
                {
                    Kvalitet = model.Kvalitet,
                    KvalitetString = model.KvalitetString,
                    Troværdighed = model.Troværdighed,
                    TroværdighedString = model.TroværdighedString,
                    Opførsel = model.Opførsel,
                    OpførselString = model.OpførselString,
                    Interaktion = model.Interaktion,
                    InteraktionString = model.InteraktionString,
                    OffentligFeedback = model.Offentligfeedback,
                    OffentligFeedbackString = model.OffentligfeedbackString,
                    Anbefaling = model.Anbefaling,
                    AnbefalingString = model.AnbefalingString,
                    ApplicationUserId = user.Id,
                    InfluenterId = model.Influenter.Id,
                    RateDateTime = DateTime.Now
                };

                // Tilføjer til EkspertRating tabellen
                _expertFeedbackRepo.Add(ekspertrating);

                // Der mangler at tjekke om denne user allerede har rated denne influenter....!!!!!!

                // Lidt feedback til brugeren
                TempData["Success"] = "Du har nu givet din ekspert anmeldelse til " + model.Influenter.Alias;


                return RedirectToAction("Show", "Influencer", new { Id = model.Influenter.Id });
            }

            TempData["Error"] = "Du skal udfylde alle felterne for at give dit feedback";



            return View("EkspertRating", model);

        }

        [HttpGet]
        public IActionResult SeAnmeldelse(int id)
        {
            var ekspertrating = _expertFeedbackRepo.Get(id);

            var model = new SeAnmeldelseViewModel()
            {
                EkspertRating = ekspertrating
            };

            return View(model);
        }

        // Update EkspertAnmelelse
        [HttpPost]
        public IActionResult SeAnmeldelse(SeAnmeldelseViewModel model)
        {
            var ekspertRating = _expertFeedbackRepo.Get(model.EkspertRating.Id);
            ekspertRating.KvalitetString = model.EkspertRating.KvalitetString;
            ekspertRating.InteraktionString = model.EkspertRating.InteraktionString;
            ekspertRating.OffentligFeedbackString = model.EkspertRating.OffentligFeedbackString;
            ekspertRating.OffentligFeedback = model.EkspertRating.OffentligFeedback;
            ekspertRating.OpførselString = model.EkspertRating.OpførselString;
            ekspertRating.TroværdighedString = model.EkspertRating.TroværdighedString;
            _expertFeedbackRepo.Update(ekspertRating);

            var ekspertrating = _expertFeedbackRepo.Get(model.EkspertRating.Id);

            var newModel = new SeAnmeldelseViewModel()
            {
                EkspertRating = ekspertrating
            };


            return View(newModel);

        }

    }
}