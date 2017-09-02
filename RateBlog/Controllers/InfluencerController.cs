using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using RateBlog.Data;
using RateBlog.Helper;
using RateBlog.Models;
using RateBlog.Models.InfluenterViewModels;
using RateBlog.Models.ManageViewModels;
using RateBlog.Repository;
using RateBlog.Services;
using RateBlog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RateBlog.Controllers
{
    public class InfluencerController : Controller
    {
        private readonly IInfluencerRepository _influencerRepo;
        private readonly IRepository<Feedback> _feedbackRepo;
        private readonly IRepository<Platform> _platformRepo;
        private readonly IRepository<Category> _categoryRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPlatformCategoryService _platformCategoryService;
        private readonly IFeedbackService _feedbackService;
        private readonly ISortService _sortService;

        public static int Counter { get; set; }

        public InfluencerController(IRepository<Category> categoryRepo, IInfluencerRepository influencer, IRepository<Feedback> feedbackRepo, UserManager<ApplicationUser> userManager, IRepository<Platform> platformRepo, IPlatformCategoryService platformCategoryService, IFeedbackService feedbackService, ISortService sortService)
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
            Dictionary<string, string> categoriesIds = new Dictionary<string, string>();
            Dictionary<string, string> platformIds = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }

            var categories = _categoryRepo.GetAll();
            var platforms = _platformRepo.GetAll();

            foreach (var v in categories)
            {
                categoriesIds.Add(v.Name, v.Id);
            }

            foreach (var v in platforms)
            {
                if (v.Name != "SecondYouTube")
                    platformIds.Add(v.Name, v.Id);
            }

            var model = new Models.InfluenterViewModels.IndexViewModel()
            {
                SearchString = search,
                CategoryIds = categoriesIds,
                PlatformIds = platformIds
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string id)
        {
            var influencer = _influencerRepo.Get(id);

            //Burde kun kunne få den pågældene user, da Index() metoden KUN returnere Users som er influenter...
            var user = await _userManager.FindByIdAsync(id);

            var gender = (user.Gender == "male") ? "Mand" : "Kvinde";

            var today = DateTime.Today;
            // Calculate the age.
            var age = today.Year - user.BirthDay.Year;
            // Go back to the year the person was born in case of a leap year
            if (user.BirthDay > today.AddYears(-age)) age--;

            var model = new ShowViewModel()
            {
                ApplicationUser = user,
                Influenter = influencer,
                Gender = gender,
                Age = age
            };

            return View(model);
        }

        [HttpGet]
        [Route("/[controller]/Feedback/[action]/{id}")]
        public async Task<IActionResult> Read(string id)
        {
            var influencer = _influencerRepo.Get(id);
            var user = await _userManager.FindByIdAsync(influencer.Id);

            var gender = (user.Gender == "male") ? "Mand" : "Kvinde";

            var today = DateTime.Today;
            // Calculate the age.
            var age = today.Year - user.BirthDay.Year;
            // Go back to the year the person was born in case of a leap year
            if (user.BirthDay > today.AddYears(-age)) age--;

            var model = new ReadViewModel()
            {
                ApplicationUser = user,
                Influenter = influencer,
                Gender = gender,
                Age = age
            };

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            var model = new CreateViewModel()
            {
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model, string[] categoriList)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                var random = RandomString.GetString(10);
                var email = "userInfluencer" + random + "@bestfluence.dk";

                user.UserName = email;
                user.Email = email;
                user.Name = model.Influenter.Alias;

                // Billede af influencer...
                if (model.ProfilePic != null)
                {
                    MemoryStream ms = new MemoryStream();
                    model.ProfilePic.OpenReadStream().CopyTo(ms);
                    user.ProfilePicture = ms.ToArray();
                }

                var result = await _userManager.CreateAsync(user, "bestfluencenewuser123321");

                if (result.Succeeded)
                {
                    // Opret en influencer
                    var newInfluenter = new Influencer();
                    newInfluenter.Alias = model.Influenter.Alias;
                    newInfluenter.Id = user.Id;
                    _influencerRepo.Add(newInfluenter);

                    var influencer = _influencerRepo.Get(user.Id);

                    foreach (var v in categoriList)
                    {
                        influencer.InfluenterKategori.Add(new InfluencerCategory() { CategoryId = v, InfluencerId = influencer.Id });
                    }

                    var platforms = _platformRepo.GetAll(); 

                    UpdatePlatform(model.FacebookLink, "Facebook", influencer, platforms);
                    UpdatePlatform(model.InstagramLink, "Instagram", influencer, platforms);
                    UpdatePlatform(model.YoutubeLink, "YouTube", influencer, platforms);
                    UpdatePlatform(model.TwitterLink, "Twitter", influencer, platforms);
                    UpdatePlatform(model.TwitchLink, "Twitch", influencer, platforms);
                    UpdatePlatform(model.WebsiteLink, "Website", influencer, platforms);
                    UpdatePlatform(model.SnapchatLink, "SnapChat", influencer, platforms);

                    _influencerRepo.SaveChanges();

                    TempData["Success"] = "Du har oprettet denne influencer. Der kan gå 24 timer før personen bliver offentlig!";
                    return RedirectToAction("Profile", new { Id = newInfluenter.Id });
                }
                else
                {
                    TempData["Error"] = "Der skete en fejl!";
                    return View(model);
                }
            }

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            var message = allErrors.First();
            TempData["Error"] = message.ErrorMessage;

            return View(model);
        }

        [HttpGet]
        [Authorize]
        [Route("/[controller]/Feedback/[action]/{id}")]
        public async Task<IActionResult> Give(string id)
        {
            var influenter = _influencerRepo.Get(id);
            var model = new GiveViewModel()
            {
                Influencer = influenter,
                Follower = await _userManager.GetUserAsync(User)
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [Route("/[controller]/Feedback/[action]/{id}")]
        public async Task<IActionResult> Give(GiveViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var influencer = _influencerRepo.Get(user.Id);

            if (influencer != null)
            {
                if (model.Influencer.Id == influencer.Id)
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

            if (ModelState.IsValid)
            {
                //var hoursSinceLastRating = _feedbackService.FeedbackCountdown(user.Id, model.Influencer.Id);
                //if (hoursSinceLastRating == 0)
                //{
                //    // Gør ingenting?? :-)
                //}
                //else if (hoursSinceLastRating < 24)
                //{
                //    var hours = TimeSpan.FromHours(24 - hoursSinceLastRating);
                //    TempData["Error"] = "Du kan anmelde denne influencer igen om " + hours.ToString(@"hh\:mm") + " minutter";
                //    return RedirectToAction("Give");
                //}

                var boolList = new List<bool>()
                {
                    model.BasedOnFacebook,
                    model.BasedOnInstagram,
                    model.BasedOnSnapchat,
                    model.BasedOnTwitch,
                    model.BasedOnTwitter,
                    model.BasedOnWebsite,
                    model.BasedOnYoutube
                };

                if (!boolList.Any(x => x == true))
                {
                    TempData["Error"] = "Du skal fortælle hvilke platforme du baseret din feedback på";
                    return RedirectToAction("Give");
                }

                if (string.IsNullOrEmpty(model.FeedbackBetter) && string.IsNullOrEmpty(model.FeedbackGood))
                {
                    TempData["Error"] = "Du skal fortælle hvad influenceren gør godt eller hvad der kan gøres bedre!";
                    return RedirectToAction("Give");
                }

                var feedback = new Feedback()
                {
                    Kvalitet = model.Kvalitet,
                    Troværdighed = model.Troværdighed,
                    Opførsel = model.Opførsel,
                    Interaktion = model.Interaktion,
                    FeedbackGood = model.FeedbackGood,
                    FeedbackBetter = model.FeedbackBetter,
                    Anbefaling = model.Anbefaling,
                    FeedbackDateTime = DateTime.Now,
                    ApplicationUserId = model.Follower.Id,
                    InfluenterId = model.Influencer.Id,
                    BasedOnSnapchat = model.BasedOnSnapchat,
                    BasedOnYoutube = model.BasedOnYoutube,
                    BasedOnFacebook = model.BasedOnFacebook,
                    BasedOnInstagram = model.BasedOnInstagram,
                    BasedOnTwitch = model.BasedOnTwitch,
                    BasedOnTwitter = model.BasedOnTwitter,
                    BasedOnWebsite = model.BasedOnWebsite,
                };
                _feedbackRepo.Add(feedback);
                TempData["Success"] = "Du har givet din feedback til " + model.Influencer.Alias;
                return RedirectToAction("Profile", "Influencer", new { Id = model.Influencer.Id });
            }


            TempData["Error"] = "Du skal udfylde alle felterne før du kan sende dit svar";
            return RedirectToAction("Give");
        }

        [HttpGet]
        public JsonResult Sorter(string[] platforme, string[] kategorier, int pageIndex, int pageSize, string search, int sortBy)
        {
            search = WebUtility.HtmlDecode(search);
            var list = _sortService.SortInfluencer(platforme, kategorier, search, sortBy);
            list = list.Where(x => _influencerRepo.Get(x.Id).IsApproved == true);
            var sortList = list.Take(pageSize * pageIndex);

            var resultList = new List<InfluencerData>();
            foreach (var v in sortList)
            {
                var influencerData = new InfluencerData()
                {
                    Id = v.Id,
                    Alias = v.Alias,
                    FeedbackCount = _feedbackService.GetFeedbackCount(v.Id, true),
                    FeedbackScore = _feedbackService.GetTotalScore(v.Id),
                };

                foreach (var cat in v.InfluenterKategori)
                {
                    if (!cat.Equals(v.InfluenterKategori.Last()))
                        influencerData.Categories += cat.Category.Name + " | ";
                    else
                        influencerData.Categories += cat.Category.Name;
                }

                foreach (var plat in v.InfluenterPlatform)
                {
                    if (plat.Platform.Name == "Facebook")
                        influencerData.Platforms += "<a href='" + plat.Link + "' target='_blank' class='fa fa-search-o fa-facebook fa-facebook-search'></a>";
                    else if (plat.Platform.Name == "YouTube")
                        influencerData.Platforms += " <a href='" + plat.Link + "' target='_blank' class='fa fa-search-o fa-youtube fa-youtube-search'></a>";
                    else if (plat.Platform.Name == "SnapChat")
                        influencerData.Platforms += " <a href='http://www.snapchat.com/add/" + plat.Link + "' target='_blank' class='fa fa-search-o fa-snapchat-ghost fa-snapchat-ghost-search'></a>";
                    else if (plat.Platform.Name == "Instagram")
                        influencerData.Platforms += " <a href='http://www.instagram.com/" + plat.Link + "' target='_blank' class='fa fa-search-o fa-instagram fa-instagram-search'></a>";
                    else if (plat.Platform.Name == "Twitter")
                        influencerData.Platforms += " <a href='http://www.twitter.com/" + plat.Link + "' target='_blank' class='fa fa-search-o fa-twitter fa-twitter-search'></a>";
                    else if (plat.Platform.Name == "SecondYouTube")
                        influencerData.Platforms += " <a href='" + plat.Link + "' target='_blank' class='fa fa-search-o fa-youtube fa-youtube-search'></a>";
                    else if (plat.Platform.Name == "Twitch")
                        influencerData.Platforms += " <a href='" + plat.Link + "' target='_blank' class='fa fa-search-o fa-twitch fa-twitch-search'></a>";
                    else if (plat.Platform.Name == "Website")
                        influencerData.Platforms += " <a href='http://" + plat.Link + "' target='_blank' class='fa fa-search-o fa-globe fa-globe-search'></a>";

                }

                resultList.Add(influencerData);
            }

            return Json(resultList);
        }

        [HttpGet]
        public JsonResult GetNextFromList(int pageIndex, int pageSize, string search, string[] platforme, string[] kategorier, int sortBy)
        {
            search = WebUtility.HtmlDecode(search);
            var sortList = _sortService.SortInfluencer(platforme, kategorier, search, sortBy);
            sortList = sortList.Where(x => _influencerRepo.Get(x.Id).IsApproved == true);
            sortList = sortList.Skip(pageIndex * pageSize).Take(pageSize);

            var resultList = new List<InfluencerData>();
            foreach (var v in sortList)
            {
                var influencerData = new InfluencerData()
                {
                    Id = v.Id,
                    Alias = v.Alias,
                    FeedbackCount = _feedbackService.GetFeedbackCount(v.Id, true),
                    FeedbackScore = _feedbackService.GetTotalScore(v.Id),
                };

                foreach (var cat in v.InfluenterKategori)
                {
                    if (!cat.Equals(v.InfluenterKategori.Last()))
                        influencerData.Categories += cat.Category.Name + " | ";
                    else
                        influencerData.Categories += cat.Category.Name;
                }

                foreach (var plat in v.InfluenterPlatform)
                {
                    if (plat.Platform.Name == "Facebook")
                        influencerData.Platforms += "<a href='" + plat.Link + "' target='_blank' class='fa fa-search-o fa-facebook fa-facebook-search'></a>";
                    if (plat.Platform.Name == "YouTube")
                        influencerData.Platforms += " <a href='" + plat.Link + "' target='_blank' class='fa fa-search-o fa-youtube fa-youtube-search'></a>";
                    if (plat.Platform.Name == "SnapChat")
                        influencerData.Platforms += " <a href='http://www.snapchat.com/add/" + plat.Link + "' target='_blank' class='fa fa-search-o fa-snapchat-ghost fa-snapchat-ghost-search'></a>";
                    if (plat.Platform.Name == "Instagram")
                        influencerData.Platforms += " <a href='http://www.instagram.com/" + plat.Link + "' target='_blank' class='fa fa-search-o fa-instagram fa-instagram-search'></a>";
                    if (plat.Platform.Name == "Twitter")
                        influencerData.Platforms += " <a href='http://www.twitter.com/" + plat.Link + "' target='_blank' class='fa fa-search-o fa-twitter fa-twitter-search'></a>";
                    if (plat.Platform.Name == "SecondYouTube")
                        influencerData.Platforms += " <a href='" + plat.Link + "' target='_blank' class='fa fa-search-o fa-youtube fa-youtube-search'></a>";
                    if (plat.Platform.Name == "Twitch")
                        influencerData.Platforms += " <a href='" + plat.Link + "' target='_blank' class='fa fa-search-o fa-twitch fa-twitch-search'></a>";
                    if (plat.Platform.Name == "Website")
                        influencerData.Platforms += " <a href='http://" + plat.Link + "' target='_blank' class='fa fa-search-o fa-globe fa-globe-search'></a>";

                }

                resultList.Add(influencerData);
            }

            return Json(resultList);
        }

        private void UpdatePlatform(string link, string name, Influencer influencer, IEnumerable<Platform> platforms)
        {
            if (!string.IsNullOrEmpty(link))
            {
                if (influencer.InfluenterPlatform.Any(x => x.PlatformId == platforms.SingleOrDefault(i => i.Name == name).Id))
                    influencer.InfluenterPlatform.SingleOrDefault(x => x.PlatformId == platforms.SingleOrDefault(i => i.Name == name).Id).Link = link;
                else
                    influencer.InfluenterPlatform.Add(new InfluencerPlatform() { InfluencerId = influencer.Id, PlatformId = platforms.SingleOrDefault(x => x.Name == name).Id, Link = link });
            }
            else
            {
                if (influencer.InfluenterPlatform.Any(x => x.PlatformId == platforms.SingleOrDefault(i => i.Name == name).Id))
                    influencer.InfluenterPlatform.Remove(influencer.InfluenterPlatform.SingleOrDefault(x => x.Platform.Name == name));
            }
        }

    }
}

