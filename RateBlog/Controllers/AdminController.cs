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
        private readonly IInfluencerRepository _influencerRepo;
        private readonly IEmailSender _emailSender;
        private readonly IPlatformCategoryService _platformCategoryService;
        private readonly IRepository<Platform> _platformRepo;
        private readonly IRepository<Category> _categoryRepo;
        private readonly IRepository<Feedback> _feedbackRepo;
        private readonly IAdminService _adminService;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public AdminController(IPasswordHasher<ApplicationUser> passwordHasher, IAdminService adminService, IRepository<Feedback> feedbackRepo, IRepository<Category> categoryRepo, IRepository<Platform> platformRepo, IEmailSender emailSender, UserManager<ApplicationUser> userManager, IInfluencerService influencerService, IInfluencerRepository influencerRepo, IPlatformCategoryService platformCategoryService)
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
        }

        [HttpGet]
        public IActionResult Index(string search)
        {
            var users = new List<ApplicationUser>();
            if (string.IsNullOrEmpty(search))
            {
                search = "";
                users = _userManager.Users.OrderByDescending(x => x.Created).Take(100).ToList();
            }
            else
            {
                var ids = _influencerRepo.GetAll().Where(x => x.Alias.ToLower().Contains(search.ToLower())).Select(x => x.Id).ToList();
                users = _userManager.Users.Where(x => x.Name.ToLower().Contains(search.ToLower()) || ids.Contains(x.Id)).ToList();
            }

            var unApprovedList = _influencerService.GetUnApprovedInfluencers();

            var model = new Models.AdminViewModels.IndexViewModel()
            {
                AllUsers = users,
                NotApprovedList = unApprovedList
            };


            return View(model);
        }

        [HttpGet]
        public IActionResult UserProfile(string id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            var influencer = _influencerRepo.Get(id);

            var editProfileViewModel = new EditProfileViewModel()
            {
                Name = user.Name,
                Email = user.Email,
                Birthday = user.BirthDay,
                Gender = user.Gender,
                Postnummer = user.Postnummer
            };

            var influencerViewModel = new InfluencerViewModel();

            if (influencer != null)
            {
                influencerViewModel = new InfluencerViewModel()
                {
                    Influencer = influencer,
                };

                PopulatePlatforms(influencer.InfluenterPlatform, influencerViewModel);
            }
            else
                influencerViewModel = null;

            var model = new UserProfileViewModel()
            {
                ApplicationUser = user,
                EditProfileViewModel = editProfileViewModel,
                InfluencerViewModel = influencerViewModel
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserProfileViewModel model, IFormFile profilePic)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == model.ApplicationUser.Id);

            user.Email = model.EditProfileViewModel.Email;
            user.UserName = model.EditProfileViewModel.Email;
            user.Name = model.EditProfileViewModel.Name;
            user.BirthDay = model.EditProfileViewModel.Birthday.Value;
            user.Postnummer = model.EditProfileViewModel.Postnummer.Value;
            user.Gender = model.EditProfileViewModel.Gender;

            if (profilePic != null)
            {
                MemoryStream ms = new MemoryStream();
                profilePic.OpenReadStream().CopyTo(ms);
                user.ProfilePicture = ms.ToArray();
            }

            var result = await _userManager.UpdateAsync(user);

            return RedirectToAction("UserProfile", new { id = model.ApplicationUser.Id });
        }

        public IActionResult EditInfluencer(UserProfileViewModel model, string[] categoriList)
        {
            var influencer = _influencerRepo.Get(model.InfluencerViewModel.Influencer.Id);

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

            UpdatePlatform(model.InfluencerViewModel.FacebookLink, "Facebook", influencer, platforms);
            UpdatePlatform(model.InfluencerViewModel.InstagramLink, "Instagram", influencer, platforms);
            UpdatePlatform(model.InfluencerViewModel.YoutubeLink, "YouTube", influencer, platforms);
            UpdatePlatform(model.InfluencerViewModel.SecoundYoutubeLink, "SecondYouTube", influencer, platforms);
            UpdatePlatform(model.InfluencerViewModel.TwitterLink, "Twitter", influencer, platforms);
            UpdatePlatform(model.InfluencerViewModel.TwitchLink, "Twitch", influencer, platforms);
            UpdatePlatform(model.InfluencerViewModel.WebsiteLink, "Website", influencer, platforms);
            UpdatePlatform(model.InfluencerViewModel.SnapchatLink, "SnapChat", influencer, platforms);

            influencer.ProfileText = model.InfluencerViewModel.Influencer.ProfileText;
            influencer.Alias = model.InfluencerViewModel.Influencer.Alias;

            _influencerRepo.SaveChanges();

            return RedirectToAction("UserProfile", new { id = model.InfluencerViewModel.Influencer.Id });

        }


        [HttpPost]
        public async Task<IActionResult> ApproveInfluencer(UserProfileViewModel model)
        {
            var influencer = _influencerRepo.Get(model.InfluencerViewModel.Influencer.Id);
            var user = _userManager.Users.SingleOrDefault(x => x.Id == model.InfluencerViewModel.Influencer.Id);
            influencer.IsApproved = true;
            _influencerRepo.Update(influencer);
            await _emailSender.SendInfluencerApprovedEmailAsync(user.Name, user.Email, influencer.Alias);
            return RedirectToAction("UserProfile", new { id = model.InfluencerViewModel.Influencer.Id });
        }

        [HttpPost]
        public async Task<IActionResult> DisapproveInfluencer(UserProfileViewModel model)
        {
            var influencer = _influencerRepo.Get(model.InfluencerViewModel.Influencer.Id);
            var user = _userManager.Users.SingleOrDefault(x => x.Id == model.InfluencerViewModel.Influencer.Id);
            _influencerRepo.Delete(influencer);

            if (user.NormalizedEmail.StartsWith("USERINFLUENCER"))
            {
                await _userManager.DeleteAsync(user);
                return RedirectToAction("Index", "Admin");
            }

            await _emailSender.SendInfluencerDisapprovedEmailAsync(user.Name, user.Email);
            return RedirectToAction("UserProfile", new { id = model.InfluencerViewModel.Influencer.Id });
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
            return RedirectToAction("UserProfile", new { id = user.Id });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(UserProfileViewModel model)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == model.ApplicationUser.Id);
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                return RedirectToAction("Index", "Admin");
            else
                return RedirectToAction("UserProfile", "Admin", new { id = model.ApplicationUser.Id });
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

            var model = new Models.AdminViewModels.FeedbackViewModel()
            {
                ApplicationUser = user,
                Influencer = influencer,
                Feedbacks = feedbackList
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult DeleteFeedback(string id, string userId)
        {
            var editFeedback = _feedbackRepo.Get(id);
            _feedbackRepo.Delete(editFeedback);

            return RedirectToAction("Feedback", new { id = userId });
        }

        [HttpGet]
        public IActionResult Statistic()
        {
            var users = _userManager.Users;

            var realUsers = users.Where(x => x.NormalizedEmail.StartsWith("USERINFLUENCER") == false).Count();

            var model = new StatisticViewModel()
            {
                FeedbackCount = _feedbackRepo.GetAll().Count(),
                UserCount = users.Count(),
                InfluencerCount = _influencerRepo.GetAll().Count(),
                RealUserCount = realUsers
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

        private void PopulatePlatforms(ICollection<InfluencerPlatform> list, InfluencerViewModel viewModel)
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
                    viewModel.SecoundYoutubeLink = v.Link;
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
    }
}
