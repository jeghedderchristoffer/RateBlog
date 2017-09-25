using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Bestfluence.Models;
using Bestfluence.Models.ManageViewModels;
using Bestfluence.Services;
using Bestfluence.Repository;
using Bestfluence.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Hosting;
using Bestfluence.Services.Interfaces;
using System.Net;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;

namespace Bestfluence.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IInfluencerRepository _influencerRepo;
        private readonly IRepository<Platform> _platformRepo;
        private readonly IRepository<Feedback> _feedbackRepo;
        private readonly IHostingEnvironment _env;
        private readonly IFeedbackService _feedbackService;
        private readonly IRepository<EmailNotification> _emailNotification;
        private readonly ApplicationDbContext _dbContext;

        public ManageController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IEmailSender emailSender,
          IInfluencerRepository influencerRepo,
          IRepository<Platform> platformRepo,
          IRepository<Category> categoryRepo,
          IRepository<Feedback> feedbackRepo,
          IHostingEnvironment env,
          IFeedbackService feedbackService,
          IRepository<EmailNotification> emailNotification,
          ApplicationDbContext dbContext)
        {
            _influencerRepo = influencerRepo;
            _platformRepo = platformRepo;
            _feedbackRepo = feedbackRepo;
            _env = env;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _feedbackService = feedbackService;
            _emailNotification = emailNotification;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            var gender = (user.Gender == "male") ? "Mand" : "Kvinde";
            // Save today's date.
            var today = DateTime.Today;
            // Calculate the age.
            var age = today.Year - user.BirthDay.Year;
            // Go back to the year the person was born in case of a leap year
            if (user.BirthDay > today.AddYears(-age)) age--;

            var model = new ProfileViewModel
            {
                ApplicationUser = user,
                Age = age,
                Gender = gender,
            };
            return View(model);
        }

        [HttpGet]
        [Route("/[controller]/Profile/[action]")]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            var influencer = _influencerRepo.Get(user.Id);

            var model = new EditViewModel
            {
                Name = user.Name,
                Email = user.Email,
                Birthday = user.BirthDay,
                Postnummer = user.Postnummer,
                Gender = user.Gender,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/[controller]/Profile/[action]")]
        public async Task<IActionResult> Edit(EditViewModel model, IFormFile profilePic)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                user.Email = model.Email;
                user.UserName = model.Email;
                user.Name = model.Name;
                user.BirthDay = model.Birthday.Value;
                user.Postnummer = model.Postnummer.Value;
                user.Gender = model.Gender;

                if (profilePic != null)
                {
                    MemoryStream ms = new MemoryStream();
                    profilePic.OpenReadStream().CopyTo(ms);
                    user.ProfilePicture = ms.ToArray();
                }

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["Success"] = "Din profil blev opdateret!";
                    return View(model);
                }

                TempData["Error"] = "Der findes allerede en bruger med denne email!";
                return View(model);
            }

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            var message = allErrors.First();
            TempData["Error"] = message.ErrorMessage;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Influencer()
        {
            var user = await GetCurrentUserAsync();
            var influencer = await _dbContext.Influencer.Include(x => x.InfluenterKategori).Include(x => x.InfluenterPlatform).ThenInclude(x => x.Platform).SingleOrDefaultAsync(x => x.Id == user.Id);
            var model = new InfluencerViewModel();

            if (influencer != null)
            {
                model.Alias = influencer.Alias;
                model.Url = influencer.Url;
                model.ProfileText = influencer.ProfileText;
                model.InfluencerCategories = influencer.InfluenterKategori;

                // PlatformLinks
                PopulatePlatforms(influencer.InfluenterPlatform, model);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Influencer(InfluencerViewModel model, string[] selectedCategories)
        {
            var user = await GetCurrentUserAsync();
            var aInfluencer = await _dbContext.Influencer.Include(x => x.InfluenterKategori).Include(x => x.InfluenterPlatform).SingleOrDefaultAsync(x => x.Id == user.Id);

            if (ModelState.IsValid)
            {
                // Add Influencer if null! 
                if (aInfluencer == null)
                {
                    var newInfluencer = new Influencer()
                    {
                        Id = user.Id,
                        Alias = model.Alias,
                    };
                    await _dbContext.Influencer.AddAsync(newInfluencer);
                    await _dbContext.SaveChangesAsync();
                }

                var influencer = await _dbContext.Influencer.Include(x => x.InfluenterKategori).Include(x => x.InfluenterPlatform).SingleOrDefaultAsync(x => x.Id == user.Id);

                // Update Categories

                foreach (var v in influencer.InfluenterKategori.ToList())
                {
                    if (!selectedCategories.Contains(v.CategoryId))
                    {
                        influencer.InfluenterKategori.Remove(v);
                    }
                }

                foreach (var v in selectedCategories)
                {
                    if (!influencer.InfluenterKategori.Any(x => x.CategoryId == v))
                    {
                        influencer.InfluenterKategori.Add(new InfluencerCategory() { CategoryId = v, InfluencerId = influencer.Id });
                    }
                }

                // Update Platforms

                var platforms = _platformRepo.GetAll();

                UpdatePlatform(model.FacebookLink, "Facebook", influencer, platforms);
                UpdatePlatform(model.InstagramLink, "Instagram", influencer, platforms);
                UpdatePlatform(model.YoutubeLink, "YouTube", influencer, platforms);
                UpdatePlatform(model.SecondYoutubeLink, "SecondYouTube", influencer, platforms);
                UpdatePlatform(model.TwitterLink, "Twitter", influencer, platforms);
                UpdatePlatform(model.TwitchLink, "Twitch", influencer, platforms);
                UpdatePlatform(model.WebsiteLink, "Website", influencer, platforms);
                UpdatePlatform(model.SnapchatLink, "SnapChat", influencer, platforms);

                influencer.ProfileText = model.ProfileText;
                influencer.Alias = model.Alias;

                // Godkend URL, Er den unik?
                if (!string.IsNullOrEmpty(model.Url))
                {
                    if (string.IsNullOrEmpty(influencer.Url))
                    {
                        if (await _dbContext.Influencer.AnyAsync(x => x.Url.ToLower() == model.Url.ToLower()))
                        {
                            TempData["Error"] = "Der findes allerede en influencer med denne URL";
                            if (influencer != null) { model.InfluencerCategories = influencer.InfluenterKategori; }
                            return View(model);
                        }
                    }
                    else if (model.Url.ToLower() != influencer.Url.ToLower())
                    {
                        if (await _dbContext.Influencer.AnyAsync(x => x.Url.ToLower() == model.Url.ToLower()))
                        {
                            TempData["Error"] = "Der findes allerede en influencer med denne URL";
                            if (influencer != null) { model.InfluencerCategories = influencer.InfluenterKategori; }
                            return View(model);
                        }
                    }

                    // URL må ikke indeholde en url vi bruger
                    if (model.Url.ToLower().StartsWith("home") ||
                        model.Url.ToLower().StartsWith("influencer") ||
                        model.Url.ToLower().StartsWith("admin") ||
                        model.Url.ToLower().StartsWith("account") ||
                        model.Url.ToLower().StartsWith("blog") ||
                        model.Url.ToLower().StartsWith("manage") ||
                        model.Url.ToLower().StartsWith("votes"))
                    {
                        TempData["Error"] = "Denne URL kan ikke bruges. Prøv en anden";
                        if (influencer != null) { model.InfluencerCategories = influencer.InfluenterKategori; }
                        return View(model);
                    }

                }

                influencer.Url = model.Url;

                _influencerRepo.SaveChanges();

                if (influencer.IsApproved)
                {
                    TempData["Success"] = "Din influencer profil er blevet opdateret!";
                }
                else
                {
                    TempData["Success"] = "Din influencer profil er blevet opdateret. Der kan gå op til 24 timer før den bliver godkendt!";
                }

                return RedirectToAction("Influencer");
            }

            if (aInfluencer != null) { model.InfluencerCategories = aInfluencer.InfluenterKategori; }

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            var message = allErrors.First();
            TempData["Error"] = message.ErrorMessage;

            return View(model);
        }

        #region Feedback 
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Feedback()
        {
            var user = await GetCurrentUserAsync();
            var influencer = _influencerRepo.Get(user.Id);

            var model = new FeedbackViewModel()
            {
                ApplicationUser = user,
                Influencer = influencer
            };

            return View(model);
        }

        [HttpGet]
        [Route("/[controller]/Feedback/[action]/{id}")]
        public IActionResult Answer(string id)
        {
            var rating = _feedbackRepo.Get(id);
            rating.ApplicationUser = _userManager.Users.SingleOrDefault(x => x.Id == rating.ApplicationUserId);

            var model = new AnswerViewModel()
            {
                Rating = rating
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AnswerFeedback(AnswerViewModel model)
        {
            var rating = _feedbackRepo.Get(model.Rating.Id);
            rating.Answer = model.Rating.Answer;
            rating.AnswerDateTime = DateTime.Now;

            _feedbackRepo.Update(rating);

            if (model.Rating.Answer == null)
            {
                TempData["Error"] = "Du fik ikke sendt dit svar.";
            }
            else
            {
                var user = _userManager.Users.SingleOrDefault(x => x.Id == rating.ApplicationUserId);
                var influencer = _influencerRepo.Get(rating.InfluenterId);

                // Send email notification 
                if (_emailNotification.Get(user.Id).FeedbackUpdate == true)
                    _emailSender.SendUserFeedbackUpdateEmailAsync(influencer.Alias, user.Email, user.Name);

                TempData["Success"] = "Du har sendt dit svar!";
            }

            return RedirectToAction("Feedback");
        }

        [HttpGet]
        [Route("/[controller]/Feedback/[action]/{id}")]
        public IActionResult Read(string id)
        {
            var rating = _feedbackRepo.Get(id);
            rating.Influenter = _influencerRepo.Get(rating.InfluenterId);

            var model = new ReadViewModel()
            {
                Rating = rating
            };

            return View(model);
        }

        [HttpGet]
        public JsonResult GetUnreadFeedback(string id)
        {
            return Json(_feedbackService.UnreadFeedbackCount(id));
        }

        [HttpPost]
        public async Task<JsonResult> ReadFeedback(string id)
        {
            var user = await GetCurrentUserAsync();
            return Json(_feedbackService.ReadFeedback(id, user.Id));
        }
        #endregion

        #region ProfilePics 
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ProfilePic()
        {
            var user = await GetCurrentUserAsync();
            byte[] buffer = user.ProfilePicture;

            if (buffer == null)
            {
                var dir = _env.WebRootPath;
                var path = Path.Combine(dir, "/images", "bfprofilepic" + ".png");
                return File(path, "image/png");
            }

            return File(buffer, "image/jpg", string.Format("{0}.jpg", user.ProfilePicture));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> UsersProfilePic(string id)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == id);

            byte[] buffer = user.ProfilePicture;

            if (buffer == null)
            {
                var dir = _env.WebRootPath;
                var path = Path.Combine(dir, "/images", "bfprofilepic" + ".png");
                return File(path, "image/png");
            }

            return File(buffer, "image/jpg", string.Format("{0}.jpg", user.ProfilePicture));
        }
        #endregion

        #region Indstillinger

        [HttpGet]
        public async Task<IActionResult> Notifications()
        {
            var user = await GetCurrentUserAsync();
            var notification = _emailNotification.Get(user.Id);

            var model = new NotificationsViewModel()
            {
                FeedbackUpdate = notification.FeedbackUpdate,
                NewsLetter = notification.NewsLetter
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Notifications(NotificationsViewModel model)
        {
            var user = await GetCurrentUserAsync();
            var emailNotificationSettings = _emailNotification.Get(user.Id);
            emailNotificationSettings.FeedbackUpdate = model.FeedbackUpdate;
            emailNotificationSettings.NewsLetter = model.NewsLetter;
            _emailNotification.Update(emailNotificationSettings);

            TempData["Success"] = "Du har ændret dine notifikations indstillinger";
            return RedirectToAction(nameof(Notifications));
        }

        [HttpGet]
        [Route("/[controller]/Change/[action]")]
        public IActionResult Password()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/[controller]/Change/[action]")]
        public async Task<IActionResult> Password(PasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["Success"] = "Du har skiftet dit kodeord!";
                    return View();
                }
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction(nameof(Profile));
        }

        #endregion

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
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
