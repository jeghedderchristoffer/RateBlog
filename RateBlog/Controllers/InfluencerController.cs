using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using RateBlog.Data;
using RateBlog.Models;
using RateBlog.Models.InfluenterViewModels;
using RateBlog.Models.ManageViewModels;
using RateBlog.Models.RatingViewModels;
using RateBlog.Repository;
using RateBlog.Services;
using RateBlog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Controllers
{
    public class InfluencerController : Controller
    {
        private readonly IRepository<Influencer> _influencerRepo;
        private readonly IRepository<Feedback> _feedbackRepo;
        private readonly IRepository<Platform> _platformRepo;
        private readonly IRepository<Category> _categoryRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPlatformCategoryService _platformCategoryService;
        private readonly IFeedbackService _feedbackService;
        private readonly ISortService _sortService;

        public InfluencerController(IRepository<Category> categoryRepo, IRepository<Influencer> influencer, IRepository<Feedback> feedbackRepo, UserManager<ApplicationUser> userManager, IRepository<Platform> platformRepo, IPlatformCategoryService platformCategoryService, IFeedbackService feedbackService, ISortService sortService)
        {
            _influencerRepo = influencer;
            _userManager = userManager;
            _feedbackRepo = feedbackRepo;
            _platformRepo = platformRepo;
            _categoryRepo = categoryRepo;
            _platformCategoryService = platformCategoryService;
            _feedbackService = feedbackService;
            _sortService = sortService;
        }

        [HttpGet]
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

            foreach (var kategori in _categoryRepo.GetAll())
            {
                if (search.ToLower().Equals(kategori.Name.ToLower()))
                {
                    influenter.AddRange(_platformCategoryService.GetAllInfluencersWithCategory(search));
                }
            }

            foreach (var platform in _platformRepo.GetAll())
            {
                if (search.ToLower().Equals(platform.Name.ToLower()))
                {
                    influenter.AddRange(_platformCategoryService.GetAllInfluencersWithPlatform(search));
                }
            }

            var model = new Models.InfluenterViewModels.IndexViewModel()
            {
                SearchString = search
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Profile(int id)
        {
            var influenter = _influencerRepo.Get(id);

            //Burde kun kunne få den pågældene user, da Index() metoden KUN returnere Users som er influenter...
            var user = _userManager.Users.SingleOrDefault(x => x.InfluenterId == id);

            var model = new ShowViewModel()
            {
                ApplicationUser = user,
                Influenter = influenter
            };

            return View(model);
        }

        [HttpGet]
        [Route("/[controller]/Feedback/[action]/{id}")]
        public IActionResult Read(int id)
        {
            var influenter = _influencerRepo.Get(id);
            var user = _userManager.Users.SingleOrDefault(x => x.InfluenterId == id);

            var model = new ReadViewModel()
            {
                ApplicationUser = user,
                Influenter = influenter
            };

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            var model = new CreateViewModel()
            {
                IKList = GetInfluenterKategoriList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Opret en influencer
                var newInfluenter = new Influencer();
                newInfluenter.Alias = model.Influenter.Alias;
                _influencerRepo.Add(newInfluenter);

                _platformCategoryService.InsertPlatform(newInfluenter.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "YouTube").Id, model.YoutubeLink);
                _platformCategoryService.InsertPlatform(newInfluenter.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "Facebook").Id, model.FacebookLink);
                _platformCategoryService.InsertPlatform(newInfluenter.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "Instagram").Id, model.InstagramLink);
                _platformCategoryService.InsertPlatform(newInfluenter.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "SnapChat").Id, model.SnapchatLink);
                _platformCategoryService.InsertPlatform(newInfluenter.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "Twitter").Id, model.TwitterLink);
                _platformCategoryService.InsertPlatform(newInfluenter.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "Website").Id, model.WebsiteLink);
                _platformCategoryService.InsertPlatform(newInfluenter.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "Twitch").Id, model.TwitchLink);

                foreach (var v in model.IKList)
                {
                    _platformCategoryService.InsertCategory(newInfluenter.Id, _categoryRepo.GetAll().SingleOrDefault(x => x.Name == v.KategoriNavn).Id, v.IsSelected);
                }

                // Lav en email baseret på deres alias... Hvis deres alias indeholde æøå vil den nok fejle...
                var email = newInfluenter.Id + "@" + newInfluenter.Id + ".dk";

                var user = new ApplicationUser();

                if (string.IsNullOrEmpty(model.Name))
                {
                    user.UserName = email;
                    user.Email = email;
                    user.Name = model.Influenter.Alias;

                    // Match applicationUser med influencer
                    user.InfluenterId = newInfluenter.Id;
                }
                else
                {
                    user.UserName = email;
                    user.Email = email;
                    user.Name = model.Name;

                    // Match applicationUser med influencer
                    user.InfluenterId = newInfluenter.Id;
                }

                // Billede af influencer...
                if (model.ProfilePic != null)
                {
                    MemoryStream ms = new MemoryStream();
                    model.ProfilePic.OpenReadStream().CopyTo(ms);
                    user.ProfilePicture = ms.ToArray();
                }

                // Koden vil være: bestfluence + InfluenterId + 123
                var result = await _userManager.CreateAsync(user, "bestfluence" + newInfluenter.Id + "123");

                if (result.Succeeded)
                {
                    TempData["Success"] = "Du har oprette denne influencer!";
                    return RedirectToAction("Profile", new { id = newInfluenter.Id });
                }
                else
                {
                    // Der findes allrede en bruger med denne email, ergo findes denne influenter, da emailen er baseret på alias...
                    // ELLER så består alias af æøå, og dette må emailen ikke være....
                    // Jeg sletter derfor influenteren igen!! 
                    _influencerRepo.Delete(newInfluenter);
                    TempData["Error"] = "Der skete en fejl!";
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        [Route("/[controller]/Feedback/[action]/{id}")]
        public IActionResult Give(int id)
        {
            var influenter = _influencerRepo.Get(id);
            var model = new RatingViewModel()
            {
                Influenter = influenter
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [Route("/[controller]/Feedback/[action]/{id}")]
        public async Task<IActionResult> Give(int kvalitet, int troværdighed, int opførsel, int interaktion, bool? anbefaling, RatingViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.InfluenterId.HasValue)
            {
                if (model.Influenter.Id == user.InfluenterId.Value)
                {
                    TempData["Error"] = "Du kan ikke anmelde dig selv!";
                    return RedirectToAction("Give");
                }
                else
                {
                    TempData["Error"] = "Influencers kan ikke anmelde andre influencers";
                    return RedirectToAction("Give");
                }
            }

            //var hoursSinceLastRating = _feedbackService.GetHoursLeftToRate(user.Id, model.Influenter.Id);

            //// Så har denne bruger ikke ratet denne influencer endnu
            //if (hoursSinceLastRating == 0)
            //{
            //    // Gør ingenting?? :-)
            //}
            //// Så har denne bruger ratet indenfor 24 timer...
            //else if (hoursSinceLastRating < 24)
            //{
            //    var hours = TimeSpan.FromHours(24 - hoursSinceLastRating);
            //    TempData["Error"] = "Du kan anmelde denne influencer igen om " + hours.ToString(@"hh\:mm") + " minutter";
            //    return RedirectToAction("Give");
            //}

            if (opførsel == 0 || kvalitet == 0 || troværdighed == 0 || interaktion == 0 || model.Review == null || anbefaling == null)
            {
                TempData["Error"] = "Du skal udfylde alle felterne for at give dit feedback";

                var errorModel = new RatingViewModel()
                {
                    Review = model.Review,
                    Influenter = model.Influenter
                };

                return View("Give", errorModel);
            }

            var rating = new Feedback()
            {
                Kvalitet = kvalitet,
                Troværdighed = troværdighed,
                Opførsel = opførsel,
                Interaktion = interaktion,
                FeedbackText = model.Review,
                Anbefaling = anbefaling,
                ApplicationUserId = user.Id,
                InfluenterId = model.Influenter.Id,
                RateDateTime = DateTime.Now
            };

            // Tilføjer til Rating tabellen
            _feedbackRepo.Add(rating);

            // Der mangler at tjekke om denne user allerede har rated denne influenter....!!!!!!

            // Lidt feedback til brugeren
            TempData["Success"] = "Du har givet din feedback til " + model.Influenter.Alias;

            // Skal ændres til Influenter Controller, ShowInfluenter Action
            return RedirectToAction("Profile", "Influencer", new { Id = model.Influenter.Id });
        }

        [HttpGet]
        public PartialViewResult Sorter(int[] platforme, int[] kategorier, int pageIndex, int pageSize, string search, int sortBy)
        {
            var list = _sortService.SortInfluencer(platforme, kategorier, search, sortBy);
            var sortList = list.Take(pageSize * pageIndex);

            return PartialView("InfluencerListPartial", sortList.ToList());
        }

        [HttpGet]
        public PartialViewResult GetNextFromList(int pageIndex, int pageSize, string search, int[] platforme, int[] kategorier, int sortBy)
        {
            var sortList = _sortService.SortInfluencer(platforme, kategorier, search, sortBy);
            sortList = sortList.Skip(pageIndex * pageSize).Take(pageSize);

            return PartialView("InfluencerListPartial", sortList.ToList());
        }

        private List<InfluenterKategoriViewModel> GetInfluenterKategoriList()
        {
            return new List<InfluenterKategoriViewModel>()
            {
                new InfluenterKategoriViewModel(){ KategoriNavn = "DIY", IsSelected = false},
                new InfluenterKategoriViewModel(){ KategoriNavn = "Beauty", IsSelected = false},
                new InfluenterKategoriViewModel(){ KategoriNavn = "Entertainment", IsSelected = false},
                new InfluenterKategoriViewModel(){ KategoriNavn = "Fashion", IsSelected = false},
                new InfluenterKategoriViewModel(){ KategoriNavn = "Food", IsSelected = false},
                new InfluenterKategoriViewModel(){ KategoriNavn = "Gaming", IsSelected = false},
                new InfluenterKategoriViewModel(){ KategoriNavn = "Lifestyle", IsSelected = false },
                new InfluenterKategoriViewModel(){ KategoriNavn = "Mommy", IsSelected = false },
                new InfluenterKategoriViewModel(){ KategoriNavn = "VLOG", IsSelected = false },
            };
        }
    }
}

