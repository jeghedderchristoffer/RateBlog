using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RateBlog.Models;
using RateBlog.Models.AdminViewModels;
using RateBlog.Models.ManageViewModels;
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
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IInfluencerService _influencerService;
        private readonly IRepository<Influencer> _influencerRepo;
        private readonly IEmailSender _emailSender;
        private readonly IPlatformCategoryService _platformCategoryService;
        private readonly IRepository<Platform> _platformRepo;
        private readonly IRepository<Category> _categoryRepo;
        private readonly IRepository<Feedback> _feedbackRepo;
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService, IRepository<Feedback> feedbackRepo, IRepository<Category> categoryRepo, IRepository<Platform> platformRepo, IEmailSender emailSender, UserManager<ApplicationUser> userManager, IInfluencerService influencerService, IRepository<Influencer> influencerRepo, IPlatformCategoryService platformCategoryService)
        {
            _userManager = userManager;
            _influencerService = influencerService;
            _influencerRepo = influencerRepo;
            _emailSender = emailSender;
            _platformCategoryService = platformCategoryService;
            _platformRepo = platformRepo;
            _categoryRepo = categoryRepo;
            _feedbackRepo = feedbackRepo;
            _adminService = adminService;
        }

        [HttpGet]
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }

            var listOfUsers = _userManager.Users.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();

            var notApprovedList = new List<ApplicationUser>();

            foreach (var v in _influencerService.GetInfluencers(listOfUsers))
            {
                if (!_influencerService.IsInfluencerApproved(v.Id))
                {
                    notApprovedList.Add(v);
                }
            }

            var model = new Models.AdminViewModels.IndexViewModel()
            {
                AllUsers = listOfUsers,
                InfluencerApprovedList = notApprovedList
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult UserProfile(string id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveInfluencer(string id)
        {
            var influencer = _influencerRepo.Get(id);
            var user = _userManager.Users.SingleOrDefault(x => x.Id == id);
            influencer.IsApproved = true;
            _influencerRepo.Update(influencer);
            await _emailSender.SendInfluencerApprovedEmailAsync(user.Name, user.Email, influencer.Alias);
            return RedirectToAction("UserProfile", new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> DisapproveInfluencer(string id)
        {
            var influencer = _influencerRepo.Get(id);
            var user = _userManager.Users.SingleOrDefault(x => x.Id == id);
            _influencerRepo.Delete(influencer);

            if (user.NormalizedEmail.StartsWith("USERINFLUENCER"))
            {
                await _userManager.DeleteAsync(user);
                return RedirectToAction("Index", "Admin");
            }

            await _emailSender.SendInfluencerDisapprovedEmailAsync(user.Name, user.Email);
            return RedirectToAction("UserProfile", new { id = id });
        }

        [HttpGet]
        public IActionResult EditUser(string id)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == id);
            var influencer = _influencerRepo.Get(id);
            var model = new EditUserViewModel();
            if (influencer != null)
            {
                model = new EditUserViewModel()
                {
                    ApplicationUser = user,
                    Influencer = influencer,
                    IKList = GetInfluenterKategoriList(id),
                    YoutubeLink = _platformCategoryService.GetPlatformLink(influencer.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "YouTube").Id),
                    FacebookLink = _platformCategoryService.GetPlatformLink(influencer.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "Facebook").Id),
                    InstagramLink = _platformCategoryService.GetPlatformLink(influencer.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "Instagram").Id),
                    SnapchatLink = _platformCategoryService.GetPlatformLink(influencer.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "SnapChat").Id),
                    TwitterLink = _platformCategoryService.GetPlatformLink(influencer.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "Twitter").Id),
                    WebsiteLink = _platformCategoryService.GetPlatformLink(influencer.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "Website").Id),
                    TwitchLink = _platformCategoryService.GetPlatformLink(influencer.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "Twitch").Id),
                };
            }
            else
            {
                model = new EditUserViewModel()
                {
                    ApplicationUser = user,
                    Influencer = influencer,
                };
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model, IFormFile pic)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == model.ApplicationUser.Id);
            user.Name = model.ApplicationUser.Name;
            user.BirthDay = model.ApplicationUser.BirthDay;
            user.Email = model.ApplicationUser.Email;
            user.Postnummer = model.ApplicationUser.Postnummer;
            user.Gender = model.ApplicationUser.Gender;
            user.PhoneNumber = model.ApplicationUser.PhoneNumber;
            user.ProfileText = model.ApplicationUser.ProfileText; 

            if (pic != null)
            {
                MemoryStream ms = new MemoryStream();
                pic.OpenReadStream().CopyTo(ms);
                user.ProfilePicture = ms.ToArray();
            }

            await _userManager.UpdateAsync(user);

            if (model.Influencer != null)
            {
                var influencer = _influencerRepo.Get(model.ApplicationUser.Id); 

                influencer.Alias = model.Influencer.Alias;
                _influencerRepo.Update(influencer);

                // Indsætter links og platforme, hvis de ikke er null. Koden skal nok laves om...

                _platformCategoryService.InsertPlatform(influencer.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "YouTube").Id, model.YoutubeLink);
                _platformCategoryService.InsertPlatform(influencer.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "Facebook").Id, model.FacebookLink);
                _platformCategoryService.InsertPlatform(influencer.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "Instagram").Id, model.InstagramLink);
                _platformCategoryService.InsertPlatform(influencer.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "SnapChat").Id, model.SnapchatLink);
                _platformCategoryService.InsertPlatform(influencer.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "Twitter").Id, model.TwitterLink);
                _platformCategoryService.InsertPlatform(influencer.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "Website").Id, model.WebsiteLink);
                _platformCategoryService.InsertPlatform(influencer.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "Twitch").Id, model.TwitchLink);


                // Insætter kategori
                foreach (var v in model.IKList)
                {
                    _platformCategoryService.InsertCategory(influencer.Id, _categoryRepo.GetAll().SingleOrDefault(x => x.Name == v.KategoriNavn).Id, v.IsSelected);
                }
            }

            return RedirectToAction("UserProfile", new { id = user.Id });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == id);
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                return RedirectToAction("Index", "Admin");
            else
                return RedirectToAction("UserProfile", "Admin", new { id = id });
        }

        public IActionResult Feedback(string id)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == id);
            var influencer = _influencerRepo.Get(id);
            List<Feedback> feedbackList = new List<Feedback>(); 

            if(influencer == null)
            {
                feedbackList.AddRange(_feedbackRepo.GetAll().Where(x => x.ApplicationUserId == id));
            }
            else
            {
                feedbackList.AddRange(_feedbackRepo.GetAll().Where(x => x.InfluenterId == influencer.Id));
            }

            var model = new FeedbackViewModel()
            {
                ApplicationUser = user, 
                Influencer = influencer, 
                Feedbacks = feedbackList
            };

            return View(model); 
        }

        [HttpPost]
        public IActionResult EditFeedback(Feedback feedback, string id, string feedbackId)
        {
            var editFeedback = _feedbackRepo.Get(feedbackId);
            editFeedback.FeedbackGood = feedback.FeedbackGood;
            editFeedback.FeedbackBetter = feedback.FeedbackBetter;
            editFeedback.Answer = feedback.Answer; 
            _feedbackRepo.Update(editFeedback);

            return RedirectToAction("Feedback", new { id = id });
        }

        [HttpPost]
        public IActionResult DeleteFeedback(string id, string feedbackId)
        {
            var editFeedback = _feedbackRepo.Get(feedbackId);
            _feedbackRepo.Delete(editFeedback);

            return RedirectToAction("Feedback", new { id = id });
        }

        [HttpGet]
        public IActionResult InfluenterStatistics(string id)
        {
            var getTheInfluenter = _userManager.Users.FirstOrDefault(x => x.Id == id);
            var listOfRatingsByTheInfluenter = _feedbackRepo.GetAll().Where(x => x.InfluenterId == getTheInfluenter.Id).ToList();

            var StatisticVm = new InfluenterStatisticsViewModel()
            {
                Influenter = getTheInfluenter,
                InfluentersFeedbacks = listOfRatingsByTheInfluenter
            };
            return View(StatisticVm);
        }


        public PartialViewResult InfluenterStatisticsBfStats(string id)
        {
            var getTheInfluenter = _userManager.Users.FirstOrDefault(x => x.Id == id);
            var listOfRatingsByTheInfluenter = _feedbackRepo.GetAll().Where(x => x.InfluenterId == getTheInfluenter.Id).ToList();

            var StatisticVm = new InfluenterStatisticsViewModel()
            {
                Influenter = getTheInfluenter,
                InfluentersFeedbacks = listOfRatingsByTheInfluenter
            };
            return PartialView("/Views/Admin/_BfStatisticsPartial.cshtml", StatisticVm);
        }

        public PartialViewResult InfluenterStatisticsTwitterStats(string id)
        {
            var getTheInfluenter = _userManager.Users.FirstOrDefault(x => x.Id == id);
            var listOfRatingsByTheInfluenter = _feedbackRepo.GetAll().Where(x => x.InfluenterId == getTheInfluenter.Id).ToList();

            var StatisticVm = new InfluenterStatisticsViewModel()
            {
                Influenter = getTheInfluenter,
                InfluentersFeedbacks = listOfRatingsByTheInfluenter
            };
            return PartialView("/Views/Admin/_TwitterStatisticsPartial.cshtml", StatisticVm);
        }

        public JsonResult PostFilter(AdminAjaxFilterViewMode data)
        {
            var feedAndUser = _adminService.GetTheFilterdFeedbacks(data.Id, data.Platform, data.AgeGroup, data.Gender);

            var ResultData = new StatisticsFilteredDataViewModel()
            {
                FilterdNumberOfUsers = _adminService.GetFilterdUniqueUsers(feedAndUser),
                FilterdNumberOfFeedbacks = _adminService.GetNumberFeedback(feedAndUser),
                FilterdTroværdighed = _adminService.GetAverageTroværdighedForFiltered(feedAndUser),
                FilterdKvalitet = _adminService.GetAverageKvalitetForFiltered(feedAndUser),
                FilterdInteraktion = _adminService.GetAverageInteraktionForFiltered(feedAndUser),
                FilterdOpførsel = _adminService.GetAverageOpførelseForFiltered(feedAndUser),
                FilterdNps = _adminService.GetNpsForFilter(feedAndUser)
            };

            return Json(ResultData);
        }



        private List<InfluenterKategoriViewModel> GetInfluenterKategoriList(string id)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == id);
            var influencer = _influencerRepo.Get(user.Id);

            if (influencer != null)
            {
                return new List<InfluenterKategoriViewModel>()
                    {
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Lifestyle", IsSelected = _platformCategoryService.IsCategorySelected(influencer.Id , _platformCategoryService.GetCategoryIdByName("Lifestyle")) },
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Beauty", IsSelected = _platformCategoryService.IsCategorySelected(influencer.Id, _platformCategoryService.GetCategoryIdByName("Beauty")) },
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Entertainment", IsSelected = _platformCategoryService.IsCategorySelected(influencer.Id, _platformCategoryService.GetCategoryIdByName("Entertainment"))  },
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Fashion", IsSelected = _platformCategoryService.IsCategorySelected(influencer.Id, _platformCategoryService.GetCategoryIdByName("Fashion")) },
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Interests", IsSelected = _platformCategoryService.IsCategorySelected(influencer.Id, _platformCategoryService.GetCategoryIdByName("Interests"))},
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Gaming", IsSelected = _platformCategoryService.IsCategorySelected(influencer.Id, _platformCategoryService.GetCategoryIdByName("Gaming"))},
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Personal", IsSelected = _platformCategoryService.IsCategorySelected(influencer.Id, _platformCategoryService.GetCategoryIdByName("Personal")) },
                    };
            }
            else
            {
                return new List<InfluenterKategoriViewModel>();
            }
        }
    }
}
