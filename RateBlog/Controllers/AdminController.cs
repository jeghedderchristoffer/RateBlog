using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RateBlog.Models;
using RateBlog.Models.AdminViewModels;
using RateBlog.Models.InfluenterViewModels;
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
        private readonly IInfluencerRepository _influencerRepo;
        private readonly IEmailSender _emailSender;
        private readonly IPlatformCategoryService _platformCategoryService;
        private readonly IRepository<Platform> _platformRepo;
        private readonly IRepository<Category> _categoryRepo;
        private readonly IRepository<Feedback> _feedbackRepo;
        private readonly IAdminService _adminService;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IRepository<ReportFeedback> _reportfeedRepo;

        public AdminController(IPasswordHasher<ApplicationUser> passwordHasher, IAdminService adminService, IRepository<Feedback> feedbackRepo, IRepository<Category> categoryRepo, IRepository<Platform> platformRepo, IEmailSender emailSender, UserManager<ApplicationUser> userManager, IInfluencerService influencerService, IInfluencerRepository influencerRepo, IPlatformCategoryService platformCategoryService, IRepository<ReportFeedback> reportfeedRepo)
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
            _passwordHasher = passwordHasher;
            _reportfeedRepo = reportfeedRepo;
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
                InfluencerApprovedList = notApprovedList,
                ReportedFeedbacks = _reportfeedRepo.GetAll().ToList()
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
                };

                PopulatePlatforms(influencer.InfluenterPlatform, model); 
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
        public async Task<IActionResult> EditUser(EditUserViewModel model, IFormFile pic, string[] categoriList)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == model.ApplicationUser.Id);
            user.Name = model.ApplicationUser.Name;
            user.BirthDay = model.ApplicationUser.BirthDay;
            user.Email = model.ApplicationUser.Email;
            user.UserName = model.ApplicationUser.Email; 
            user.Postnummer = model.ApplicationUser.Postnummer;
            user.Gender = model.ApplicationUser.Gender;
            user.PhoneNumber = model.ApplicationUser.PhoneNumber;

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

                foreach (var v in influencer.InfluenterKategori.ToList())
                {
                    if (!categoriList.Contains(v.CategoryId))
                    {
                        influencer.InfluenterKategori.Remove(v);
                    }
                }


                foreach (var v in categoriList)
                {
                    if (!influencer.InfluenterKategori.Any(x => x.CategoryId == v))
                    {
                        influencer.InfluenterKategori.Add(new InfluencerCategory() { CategoryId = v, InfluencerId = influencer.Id });
                    }
                }

                var platforms = _platformRepo.GetAll();

                UpdatePlatform(model.FacebookLink, "Facebook", influencer, platforms);
                UpdatePlatform(model.InstagramLink, "Instagram", influencer, platforms);
                UpdatePlatform(model.YoutubeLink, "YouTube", influencer, platforms);
                UpdatePlatform(model.SecondYoutubeLink, "SecondYouTube", influencer, platforms);
                UpdatePlatform(model.TwitterLink, "Twitter", influencer, platforms);
                UpdatePlatform(model.TwitchLink, "Twitch", influencer, platforms);
                UpdatePlatform(model.WebsiteLink, "Website", influencer, platforms);
                UpdatePlatform(model.SnapchatLink, "SnapChat", influencer, platforms);

                influencer.ProfileText = model.Influencer.ProfileText;
                influencer.Alias = model.Influencer.Alias;

                _influencerRepo.SaveChanges();


            }

            return RedirectToAction("UserProfile", new { id = user.Id });
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id); 
            return View(user); 
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string id, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
            var result = await _userManager.UpdateAsync(user);

            return RedirectToAction("Index"); 
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

        [HttpGet]
        public IActionResult Feedback(string id)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == id);
            var influencer = _influencerRepo.Get(id);
            List<Feedback> feedbackList = new List<Feedback>();

            if (influencer == null)
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

        [HttpGet]
        public IActionResult EditFeedback(string id, bool isInfluencer)
        {
            var feedback = _feedbackRepo.Get(id);

            return View(feedback); 
        }

        [HttpPost]
        public IActionResult EditFeedback(Feedback feedback)
        {
            var editFeedback = _feedbackRepo.Get(feedback.Id);
            editFeedback.FeedbackGood = feedback.FeedbackGood;
            editFeedback.FeedbackBetter = feedback.FeedbackBetter;
            editFeedback.Answer = feedback.Answer;
            _feedbackRepo.Update(editFeedback);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteFeedback(Feedback feedback)
        {
            var editFeedback = _feedbackRepo.Get(feedback.Id);
            _feedbackRepo.Delete(editFeedback);

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
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

        
        [AllowAnonymous]
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

        [AllowAnonymous]
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

        private void PopulatePlatforms(ICollection<InfluencerPlatform> list, EditUserViewModel viewModel)
        {
            foreach (var v in list)
            {
                if (v.Platform.Name == "Facebook")
                {
                    viewModel.FacebookLink = v.Link;
                }
                else if (v.Platform.Name == "YouTube")
                {
                    viewModel.YoutubeLink = v.Link;
                }
                else if (v.Platform.Name == "SecondYouTube")
                {
                    viewModel.SecondYoutubeLink = v.Link;
                }
                else if (v.Platform.Name == "Twitter")
                {
                    viewModel.TwitterLink = v.Link;
                }
                else if (v.Platform.Name == "Twitch")
                {
                    viewModel.TwitchLink = v.Link;
                }
                else if (v.Platform.Name == "Website")
                {
                    viewModel.WebsiteLink = v.Link;
                }
                else if (v.Platform.Name == "Instagram")
                {
                    viewModel.InstagramLink = v.Link;
                }
                else if (v.Platform.Name == "SnapChat")
                {
                    viewModel.SnapchatLink = v.Link;
                }
            }
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


        [AllowAnonymous]
        [HttpPost]
        public IActionResult ReportTest(ReportFeedback newFeedback)
        {
            _reportfeedRepo.Add(newFeedback);
            
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> ReportFeedbackJson(ReportFeedback newFeedback)
        {
            string reportingUserId = (await _userManager.GetUserAsync(HttpContext.User))?.Id;
            string status = "";

            if (!_reportfeedRepo.GetAll().Where(x => x.ApplicationUserId == newFeedback.ApplicationUserId).Where(x => x.TheUserWhoReportedId == reportingUserId).Where(x=>x.FeedbackId == newFeedback.FeedbackId).Any())
            {
                newFeedback.TheUserWhoReportedId = reportingUserId;
                newFeedback.ReportedDateTime = DateTime.Now;
                _reportfeedRepo.Add(newFeedback);
                status = "Sucess";
            }
            else
            {
                status = "Failure";
            }
            return Json(status);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ManageReportedFeedback(string Id)
        {
            var report = _reportfeedRepo.Get(Id);
            var reportedUsr = _userManager.Users.FirstOrDefault(x => x.Id == report.ApplicationUserId);
            var reportedFeed = _feedbackRepo.GetAll().FirstOrDefault(x=> x.Id == report.FeedbackId);
            var reportFeedbackList = _reportfeedRepo.GetAll().Where(x => x.FeedbackId == report.FeedbackId).ToList();

            var ReportFeedback = new ReportFeedbackViewModel
            {
                TheReportedFeedback = reportedFeed,
                Report=report,
                TheReportedUser=reportedUsr,
                ReportFeedbacks=reportFeedbackList
            };

            return View(ReportFeedback);
        }

        [HttpPost]
        public IActionResult EditReport()
        {
            return View();
        }


        [HttpPost]
        public IActionResult DeleteFeedbackAndReport(ReportFeedbackViewModel Reportfeedback)
        {
            var reportsFeedbacks = _reportfeedRepo.GetAll().Where(x=>x.FeedbackId == Reportfeedback.Report.FeedbackId);

            var editFeedback = _feedbackRepo.Get(Reportfeedback.Report.FeedbackId);

            foreach(var x in reportsFeedbacks.ToList())
            {
                _reportfeedRepo.Delete(x);
            }
            _feedbackRepo.Delete(editFeedback);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SvaretReport(ReportFeedbackViewModel Reportfeedback)
        {
            var reportsFeedbacks = _reportfeedRepo.GetAll().Where(x => x.FeedbackId == Reportfeedback.Report.FeedbackId);

            foreach (var x in reportsFeedbacks.ToList())
            {
                x.IsRead = true;
                _reportfeedRepo.Update(x);
            }

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public JsonResult InstagramAPITest()
        {
            return Json("");
        }

        [AllowAnonymous]
        public IActionResult Test(string access_token)
        {
            return View(access_token);
        }


    }
}
