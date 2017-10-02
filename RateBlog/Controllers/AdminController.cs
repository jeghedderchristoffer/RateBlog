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
using RateBlog;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Bestfluence.Data;
using System.Net;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
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
        private readonly ApplicationDbContext _context;

        public AppKeyConfig AppConfigs { get; }

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
            IOptions<AppKeyConfig> appkeys)
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
            _context = context;
            AppConfigs = appkeys.Value;
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
        [AllowAnonymous]
        [HttpGet]
        public IActionResult InfluenterStatistics(string id)
        {
            var getTheInfluenter = _userManager.Users.FirstOrDefault(x => x.Id == id);
            var listOfRatingsByTheInfluenter = _feedbackRepo.GetAll().Where(x => x.InfluenterId == getTheInfluenter.Id).ToList();
            var GetTheInfuenterAsInfluenter = _influencerRepo.GetAll().FirstOrDefault(x => x.Id == getTheInfluenter.Id);
            var HasInstaData = _context.InstagramData.Any(x => x.InfluencerId == id);

            var StatisticVm = new InfluenterStatisticsViewModel()
            {
                HasInstagramData = HasInstaData,
                InfluenterUserInfo = getTheInfluenter,
                InfluentersFeedbacks = listOfRatingsByTheInfluenter,
                Influenter = GetTheInfuenterAsInfluenter
            };

            return View(StatisticVm);
        }

        public dynamic[] GetInstagramFacebookData(string code)
        {
            string InstagramBusinessId = "";
            dynamic Access_Token = "";

            dynamic[] AllDatas = new dynamic[3];
            //Authenticate (Get AcessToken)
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri("https://graph.facebook.com/oauth/");
                HttpResponseMessage Access_Token_Response = client.GetAsync("access_token?client_id=" + AppConfigs.AppId + "&client_secret=" + AppConfigs.AppSecret + "&code=" + code + "&redirect_uri=http://localhost:54069/Admin/StatisticsInstagramAccounts").Result;
                Access_Token_Response.EnsureSuccessStatusCode();
                string Access_Token_Result = Access_Token_Response.Content.ReadAsStringAsync().Result;
                Access_Token = JObject.Parse(Access_Token_Result);
                Access_Token = Access_Token.access_token;
            }
            //Get Business Id
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri("https://graph.facebook.com/v2.10/");
                HttpResponseMessage BusinessId_Response = client.GetAsync("me/accounts?fields=instagram_business_account&redirect_uri=http://localhost:54069/Admin/StatisticsInstagramAccounts&access_token=" + Access_Token).Result;
                BusinessId_Response.EnsureSuccessStatusCode();
                string BusinessId_Result = BusinessId_Response.Content.ReadAsStringAsync().Result;
                dynamic Business_Data = JObject.Parse(BusinessId_Result);
                foreach (var i in Business_Data.data)
                {
                    if (i.instagram_business_account != null)
                    {
                        if (InstagramBusinessId != "")
                        {
                            //then he/she has a secondacc so redirect back to page and ask which it is
                        }
                        InstagramBusinessId = (string)i.instagram_business_account.id;
                    }
                }
            }
            //Get Instagram MetaData
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri("https://graph.facebook.com/v2.10/");
                HttpResponseMessage MetaData_Response = client.GetAsync(InstagramBusinessId + "?fields=followers_count,media_count&redirect_uri=http://localhost:54069/Admin/StatisticsInstagramAccounts&access_token=" + Access_Token).Result;
                MetaData_Response.EnsureSuccessStatusCode();
                string MetaData_Result = MetaData_Response.Content.ReadAsStringAsync().Result;
                AllDatas[0] = JObject.Parse(MetaData_Result);
            }
            //Get Instagram Lifetime Data
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri("https://graph.facebook.com/v2.10/");
                HttpResponseMessage LifeTime_Response = client.GetAsync(InstagramBusinessId + "/insights?metric=audience_gender_age,audience_country,audience_city&period=lifetime&redirect_uri=http://localhost:54069/Admin/StatisticsInstagramAccounts&access_token=" + Access_Token).Result;
                LifeTime_Response.EnsureSuccessStatusCode();
                string LifeTime_Result = LifeTime_Response.Content.ReadAsStringAsync().Result;
                AllDatas[1] = JObject.Parse(LifeTime_Result);
            }
            //Get all Instagram day,week and month info
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri("https://graph.facebook.com/v2.10/");
                HttpResponseMessage Reach_Impression_Response = client.GetAsync(InstagramBusinessId + "/insights?metric=impressions,reach&period=day,week,days_28&redirect_uri=http://localhost:54069/Admin/StatisticsInstagramAccounts&access_token=" + Access_Token).Result;
                Reach_Impression_Response.EnsureSuccessStatusCode();
                string Reach_Impression_Result = Reach_Impression_Response.Content.ReadAsStringAsync().Result;
                AllDatas[2] = JObject.Parse(Reach_Impression_Result);
            }
            return AllDatas;
        }

        public async Task InstagramDataAdd(string code)
        {
            var AllDatas = GetInstagramFacebookData(code);
            string UserId = (await _userManager.GetUserAsync(User)).Id;
            InstagramData InstagramDatas = new InstagramData { InfluencerId = UserId };
            InstagramDatas.LastUpdated = DateTime.Now;
            List<InstagramCountry> InstagramCountryList = new List<InstagramCountry>();
            List<InstagramCity> InstagramCityList = new List<InstagramCity>();
            InstagramDatas.MediaCount = AllDatas[0].media_count;
            InstagramDatas.FollowerCount = AllDatas[0].followers_count;
            var InstagramAgeGroups = new InstagramAgeGroup()
            {
                Female13To17 = AllDatas[1].data[0].values[0].value["F.13-17"] ?? 0,
                Female18To24 = AllDatas[1].data[0].values[0].value["F.18-24"] ?? 0,
                Female25To34 = AllDatas[1].data[0].values[0].value["F.25-34"] ?? 0,
                Female35To44 = AllDatas[1].data[0].values[0].value["F.35-44"] ?? 0,
                Female45To55 = AllDatas[1].data[0].values[0].value["F.45-54"] ?? 0,
                Female55To64 = AllDatas[1].data[0].values[0].value["F.55-64"] ?? 0,
                Female65Plus = AllDatas[1].data[0].values[0].value["F.65+"] ?? 0,
                Male13To17 = AllDatas[1].data[0].values[0].value["M.13-17"] ?? 0,
                Male18To24 = AllDatas[1].data[0].values[0].value["M.18-24"] ?? 0,
                Male25To34 = AllDatas[1].data[0].values[0].value["M.25-34"] ?? 0,
                Male35To44 = AllDatas[1].data[0].values[0].value["M.35-44"] ?? 0,
                Male45To55 = AllDatas[1].data[0].values[0].value["M.45-54"] ?? 0,
                Male55To64 = AllDatas[1].data[0].values[0].value["M.55-64"] ?? 0,
                Male65Plus = AllDatas[1].data[0].values[0].value["M.65+"] ?? 0
            };
            InstagramDatas.InstagramAgeGroup = InstagramAgeGroups;

            var countries = _context.Country.ToList();
            foreach (var i in AllDatas[1].data[1].values[0].value)
            {
                if (!countries.Any(x => x.Name == i.Name))
                {
                    _context.Country.Add(new Country { Name = i.Name });
                    _context.SaveChanges();
                    countries = _context.Country.ToList();
                }
                var TheCountry = countries.FirstOrDefault(x => x.Name == (string)i.Name);
                var InstaCountry = new InstagramCountry()
                {
                    InstagramData = InstagramDatas,
                    InstagramDataId = InstagramDatas.Id,
                    CountryId = TheCountry.Id,
                    Country = TheCountry,
                    Count = i.Value
                };
                InstagramCountryList.Add(InstaCountry);
            }
            InstagramDatas.InstagramCountry = InstagramCountryList;

            var cities = _context.City.ToList();
            foreach (var i in AllDatas[1].data[2].values[0].value)
            {
                if (!cities.Any(x => x.Name == i.Name))
                {
                    _context.City.Add(new City { Name = i.Name });
                    _context.SaveChanges();
                    cities = _context.City.ToList();
                }
                var TheCity = cities.FirstOrDefault(x => x.Name == (string)i.Name);
                var InstaCity = new InstagramCity()
                {
                    InstagramData = InstagramDatas,
                    InstagramDataId = InstagramDatas.Id,
                    CityId = TheCity.Id,
                    City = TheCity,
                    Count = i.Value
                };
                InstagramCityList.Add(InstaCity);
            }
            InstagramDatas.InstagramCity = InstagramCityList;
            InstagramDatas.DayImpression = AllDatas[2].data[0].values[0].value ?? 0;
            InstagramDatas.WeekImpression = AllDatas[2].data[1].values[0].value ?? 0;
            InstagramDatas.MonthImpression = AllDatas[2].data[2].values[0].value ?? 0;
            InstagramDatas.DayReach = AllDatas[2].data[3].values[0].value ?? 0;
            InstagramDatas.WeekReach = AllDatas[2].data[4].values[0].value ?? 0;
            InstagramDatas.MonthReach = AllDatas[2].data[5].values[0].value ?? 0;
            _context.InstagramData.Add(InstagramDatas);
            _context.SaveChanges();
        }

        public async Task InstagramDataUpdate(string code)
        {
            string UserId = (await _userManager.GetUserAsync(User)).Id;
            InstagramData InstagramDatas = _context.InstagramData.Include(x => x.InstagramAgeGroup).Include(x => x.InstagramCity).ThenInclude(x => x.City).Include(x => x.InstagramCountry).ThenInclude(x => x.Country).SingleOrDefault(x => x.InfluencerId == UserId);
            InstagramDatas.LastUpdated = DateTime.Now;
            var AllDatas = GetInstagramFacebookData(code);
            //MetaData
            InstagramDatas.MediaCount = AllDatas[0].followers_count;
            InstagramDatas.FollowerCount = AllDatas[0].media_count;
            //LifeTimeData
            InstagramDatas.InstagramAgeGroup.InstagramDataId = InstagramDatas.Id;
            InstagramDatas.InstagramAgeGroup.InstagramData = InstagramDatas;
            InstagramDatas.InstagramAgeGroup.Female13To17 = AllDatas[1].data[0].values[0].value["F.13-17"] ?? 0;
            InstagramDatas.InstagramAgeGroup.Female18To24 = AllDatas[1].data[0].values[0].value["F.18-24"] ?? 0;
            InstagramDatas.InstagramAgeGroup.Female25To34 = AllDatas[1].data[0].values[0].value["F.25-34"] ?? 0;
            InstagramDatas.InstagramAgeGroup.Female35To44 = AllDatas[1].data[0].values[0].value["F.35-44"] ?? 0;
            InstagramDatas.InstagramAgeGroup.Female45To55 = AllDatas[1].data[0].values[0].value["F.45-54"] ?? 0;
            InstagramDatas.InstagramAgeGroup.Female55To64 = AllDatas[1].data[0].values[0].value["F.55-64"] ?? 0;
            InstagramDatas.InstagramAgeGroup.Female65Plus = AllDatas[1].data[0].values[0].value["F.65+"] ?? 0;
            InstagramDatas.InstagramAgeGroup.Male13To17 = AllDatas[1].data[0].values[0].value["M.13-17"] ?? 0;
            InstagramDatas.InstagramAgeGroup.Male18To24 = AllDatas[1].data[0].values[0].value["M.18-24"] ?? 0;
            InstagramDatas.InstagramAgeGroup.Male25To34 = AllDatas[1].data[0].values[0].value["M.25-34"] ?? 0;
            InstagramDatas.InstagramAgeGroup.Male35To44 = AllDatas[1].data[0].values[0].value["M.35-44"] ?? 0;
            InstagramDatas.InstagramAgeGroup.Male45To55 = AllDatas[1].data[0].values[0].value["M.45-54"] ?? 0;
            InstagramDatas.InstagramAgeGroup.Male55To64 = AllDatas[1].data[0].values[0].value["M.55-64"] ?? 0;
            InstagramDatas.InstagramAgeGroup.Male65Plus = AllDatas[1].data[0].values[0].value["M.65+"] ?? 0;
            //day,week,month
            var countries = _context.Country.ToList();
            foreach (var i in AllDatas[1].data[1].values[0].value)
            {
                if (!countries.Any(x => x.Name == i.Name))
                {
                    _context.Country.Add(new Country { Name = i.Name });
                    _context.SaveChanges();
                    countries = _context.Country.ToList();
                }
                var TheCountry = countries.FirstOrDefault(x => x.Name == (string)i.Name);
                if (InstagramDatas.InstagramCountry.FirstOrDefault(x => x.CountryId == TheCountry.Id) == null)
                {
                    var InstaCountry = new InstagramCountry()
                    {
                        InstagramData = InstagramDatas,
                        InstagramDataId = InstagramDatas.Id,
                        CountryId = TheCountry.Id,
                        Country = TheCountry,
                        Count = i.Value
                    };
                    InstagramDatas.InstagramCountry.Add(InstaCountry);
                }
                else
                {
                    InstagramDatas.InstagramCountry.FirstOrDefault(x => x.CountryId == TheCountry.Id).Count = i.Value;
                }
            }
            var cities = _context.City.ToList();
            foreach (var i in AllDatas[1].data[2].values[0].value)
            {
                if (!cities.Any(x => x.Name == i.Name))
                {
                    _context.City.Add(new City { Name = i.Name });
                    _context.SaveChanges();
                    cities = _context.City.ToList();
                }
                var TheCity = cities.FirstOrDefault(x => x.Name == (string)i.Name);
                if (InstagramDatas.InstagramCity.FirstOrDefault(x => x.CityId == TheCity.Id) == null)
                {
                    var InstaCity = new InstagramCity()
                    {
                        InstagramData = InstagramDatas,
                        InstagramDataId = InstagramDatas.Id,
                        CityId = TheCity.Id,
                        City = TheCity,
                        Count = i.Value
                    };
                    InstagramDatas.InstagramCity.Add(InstaCity);
                }
                else
                {
                    InstagramDatas.InstagramCity.FirstOrDefault(x => x.CityId == TheCity.Id).Count = i.Value;
                }
            }
            InstagramDatas.DayImpression = AllDatas[2].data[0].values[0].value ?? 0;
            InstagramDatas.WeekImpression = AllDatas[2].data[1].values[0].value ?? 0;
            InstagramDatas.MonthImpression = AllDatas[2].data[2].values[0].value ?? 0;
            InstagramDatas.DayReach = AllDatas[2].data[3].values[0].value ?? 0;
            InstagramDatas.WeekReach = AllDatas[2].data[4].values[0].value ?? 0;
            InstagramDatas.MonthReach = AllDatas[2].data[5].values[0].value ?? 0;
            _context.InstagramData.Update(InstagramDatas);
            _context.SaveChanges();
        }

        [AllowAnonymous]
        public async Task<IActionResult> StatisticsInstagramAccounts(string code)
        {
            string UserId = (await _userManager.GetUserAsync(User)).Id;
            if (_context.InstagramData.Any(x => x.InfluencerId == UserId))
            {
                await InstagramDataUpdate(code);
            }
            else
            {
                await InstagramDataAdd(code);
            }
            return RedirectToAction("InfluenterStatistics", new { id = UserId });
        }

        [AllowAnonymous]
        public JsonResult InstagramData(string id)
        {
            var GetInstagramData = _context.InstagramData.
                Include(x => x.InstagramAgeGroup).
                Include(x => x.InstagramCity).
                ThenInclude(x => x.City).Include(x => x.InstagramCountry).
                ThenInclude(x => x.Country).
                SingleOrDefault(x => x.InfluencerId == id);

            GetInstagramData.InstagramAgeGroup.InstagramData = null;
            GetInstagramData.Influencer = null;
            GetInstagramData.InstagramCity.Select(x => { x.InstagramData = null; return x; }).ToList();
            GetInstagramData.InstagramCountry.Select(x => { x.InstagramData = null; return x; }).ToList();

            return Json(GetInstagramData);
        }

        [AllowAnonymous]
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
