using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Bestfluence.Helper;
using Bestfluence.Models;
using Bestfluence.Models.AdminViewModels;
using Bestfluence.Models.ManageViewModels;
using Bestfluence.Repository;
using Bestfluence.Services;
using Bestfluence.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bestfluence.Data;
using Microsoft.EntityFrameworkCore;

namespace Bestfluence.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IInfluencerService _influencerService;
        private readonly IInfluencerRepository _influencerRepo;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<Platform> _platformRepo;
        private readonly IRepository<Category> _categoryRepo;
        private readonly IRepository<Feedback> _feedbackRepo;
        private readonly IAdminService _adminService;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IFeedbackService _feedbackService;
        private readonly IRepository<FeedbackReport> _feedbackReportRepo;
        private readonly IRepository<BlogArticle> _blogRepo;
        private readonly ApplicationDbContext _dbContext;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AdminController(IRepository<FeedbackReport> feedbackReportRepo,
            IFeedbackService feedbackService,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IAdminService adminService,
            IRepository<Feedback> feedbackRepo,
            IRepository<Category> categoryRepo,
            IRepository<Platform> platformRepo,
            IEmailSender emailSender,
            UserManager<ApplicationUser> userManager,
            IInfluencerService influencerService,
            IInfluencerRepository influencerRepo,
            IRepository<BlogArticle> blogRepo,
            ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _influencerService = influencerService;
            _influencerRepo = influencerRepo;
            _emailSender = emailSender;
            _platformRepo = platformRepo;
            _categoryRepo = categoryRepo;
            _feedbackRepo = feedbackRepo;
            _adminService = adminService;
            _passwordHasher = passwordHasher;
            _feedbackService = feedbackService;
            _feedbackReportRepo = feedbackReportRepo;
            _blogRepo = blogRepo;
            _dbContext = context;
            _signInManager = signInManager;
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
            var feedbackReports = _feedbackService.GetUnreadFeedbackReports();

            var feedbackList = new List<DisplayFeedbackReports>();

            foreach (var v in feedbackReports)
            {
                if (!feedbackList.Any(x => x.Feedback == v.Feedback))
                {
                    feedbackList.Add(new DisplayFeedbackReports() { Feedback = v.Feedback, Count = 1 });
                }
                else
                {
                    feedbackList.SingleOrDefault(x => x.Feedback == v.Feedback).Count++;
                }
            }

            var model = new Models.AdminViewModels.IndexViewModel()
            {
                AllUsers = users,
                NotApprovedList = unApprovedList,
                FeedbackReports = feedbackList
            };


            return View(model);
        }

        [HttpGet]
        public IActionResult UserProfile(string id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            var influencer = _influencerRepo.Get(id);

            var editProfileViewModel = new EditViewModel()
            {
                Name = user.Name,
                Email = user.Email,
                Birthday = user.BirthDay,
                Gender = user.Gender,
                Postnummer = user.Postnummer
            };

            var influencerViewModel = new Models.AdminViewModels.InfluencerViewModel();

            if (influencer != null)
            {
                influencerViewModel = new Models.AdminViewModels.InfluencerViewModel()
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

        [HttpPost]
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
            UpdatePlatform(model.InfluencerViewModel.SecondYoutubeLink, "SecondYouTube", influencer, platforms);
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

            await _userManager.AddToRoleAsync(user, "Influencer");

            await _signInManager.RefreshSignInAsync(user); 

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
            var user = await _dbContext.Users
                .Include(x => x.Feedbacks)
                .Include(x => x.FeedbackReports)
                .Include(x => x.EmailNotification)
                .Include(x => x.BlogComments)
                .Include(x => x.BlogRatings)
                .Include(x => x.VoteAnswers)
                .Include(x => x.Influencer).ThenInclude(x => x.Votes).ThenInclude(x=> x.VoteQuestions).SingleOrDefaultAsync(x => x.Id == model.ApplicationUser.Id);

            foreach(var v in user.Feedbacks.ToList())
            {
                _dbContext.Feedback.Remove(v); 
            }

            foreach(var v in user.FeedbackReports.ToList())
            {
                _dbContext.FeedbackReports.Remove(v); 
            }

            foreach(var v in user.BlogComments.ToList())
            {
                _dbContext.BlogComments.Remove(v); 
            }

            foreach(var v in user.BlogRatings.ToList())
            {
                _dbContext.BlogRatings.Remove(v); 
            }

            foreach(var v in user.VoteAnswers.ToList())
            {
                _dbContext.VoteAnswers.Remove(v); 
            }

            if(user.Influencer != null)
            {
                foreach(var v in user.Influencer.Votes.Select(x => x.VoteQuestions).Select(x => x).ToList())
                {
                    foreach(var i in v)
                    {
                        _dbContext.VoteQuestions.Remove(i); 
                    }
                }

                foreach(var v in user.Influencer.Votes)
                {
                    _dbContext.Votes.Remove(v);
                }

                _dbContext.Influencer.Remove(user.Influencer);
            }
            await _dbContext.SaveChangesAsync();
            await _userManager.DeleteAsync(user); 
            return RedirectToAction("Index", "Admin");
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
        public IActionResult FeedbackReports(string id)
        {
            var feedback = _feedbackService.GetFeedbackInfo(id);
            var reports = _feedbackService.GetReportForFeedback(feedback.Id);

            var model = new FeedbackReportViewModel()
            {
                Feedback = feedback,
                FeedbackReports = reports
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult FeedbackReportsOk(string id)
        {
            var reports = _feedbackService.GetReportForFeedback(id);

            foreach (var v in reports.ToList())
            {
                v.IsRead = true;
                _feedbackReportRepo.Update(v);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult FeedbackReportsDelete(string id)
        {
            var reports = _feedbackService.GetReportForFeedback(id);

            foreach (var v in reports.ToList())
            {
                _feedbackReportRepo.Delete(v);
            }

            _feedbackRepo.Delete(_feedbackRepo.Get(id));

            return RedirectToAction("Index");
        }

        #region Blog

        [HttpGet]
        public IActionResult BlogArticles()
        {
            var articles = _blogRepo.GetAll();
            var model = new BlogArticlesViewModel()
            {
                Articles = articles
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateArticle()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new CreateArticlesViewModel()
            {
                Author = user.Name
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateArticle(CreateArticlesViewModel model)
        {
            if (ModelState.IsValid)
            {
                BlogArticle blog = new BlogArticle()
                {
                    Title = model.Title,
                    DateTime = model.DateTime,
                    Author = model.Author,
                    Categories = model.Categories,
                    Description = model.Description,
                    ArticleText = model.ArticleText,
                    Url = model.Url
                };

                if (model.IndexPicture != null)
                {
                    MemoryStream ms = new MemoryStream();
                    model.IndexPicture.OpenReadStream().CopyTo(ms);
                    blog.IndexPicture = ms.ToArray();
                }

                if (model.ArticlePicture != null)
                {
                    MemoryStream ms = new MemoryStream();
                    model.ArticlePicture.OpenReadStream().CopyTo(ms);
                    blog.ArticlePicture = ms.ToArray();
                }
                _blogRepo.Add(blog);
                return RedirectToAction("BlogArticles");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult EditArticle(string id)
        {
            var article = _blogRepo.Get(id);
            var model = new EditArticleViewModel()
            {
                Id = article.Id,
                ArticleText = article.ArticleText,
                Author = article.Author,
                Categories = article.Categories,
                DateTime = article.DateTime,
                Description = article.Description,
                Title = article.Title,
                Url = article.Url
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult EditArticle(EditArticleViewModel model)
        {
            var article = _blogRepo.Get(model.Id);
            article.Title = model.Title;
            article.DateTime = model.DateTime;
            article.Description = model.Description;
            article.Categories = model.Categories;
            article.Author = model.Author;
            article.ArticleText = model.ArticleText;
            article.Url = model.Url;

            if (model.IndexPicture != null)
            {
                MemoryStream ms = new MemoryStream();
                model.IndexPicture.OpenReadStream().CopyTo(ms);
                article.IndexPicture = ms.ToArray();
            }

            if (model.ArticlePicture != null)
            {
                MemoryStream ms = new MemoryStream();
                model.ArticlePicture.OpenReadStream().CopyTo(ms);
                article.ArticlePicture = ms.ToArray();
            }

            _blogRepo.Update(article);

            return RedirectToAction("BlogArticles");
        }

        [HttpPost]
        public IActionResult DeleteArticle(string id)
        {
            var article = _blogRepo.Get(id);
            _blogRepo.Delete(article);
            return RedirectToAction("BlogArticles");
        }

        [HttpPost]
        public IActionResult PublishArticle(string id)
        {
            var article = _blogRepo.Get(id);
            if (article.Publish)
            {
                article.Publish = false;
            }
            else
            {
                article.Publish = true;
            }
            _blogRepo.Update(article);

            return RedirectToAction("BlogArticles");
        }

        [HttpGet]
        public IActionResult ArticlePreview(string id)
        {
            var article = _blogRepo.Get(id);
            return View(article);
        }

        #endregion

        #region Thomas
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
        #endregion

        #region Helpers

        private void PopulatePlatforms(ICollection<InfluencerPlatform> list, Models.AdminViewModels.InfluencerViewModel viewModel)
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

        #endregion

    }
}
