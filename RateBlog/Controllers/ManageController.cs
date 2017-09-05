using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RateBlog.Models;
using RateBlog.Models.ManageViewModels;
using RateBlog.Services;
using RateBlog.Repository;
using RateBlog.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Hosting;
using RateBlog.Services.Interfaces;
using System.Net;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;

namespace RateBlog.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly string _externalCookieScheme;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;


        private readonly IInfluencerRepository _influencerRepo;
        private readonly IRepository<Platform> _platformRepo;
        private readonly IRepository<Category> _categoryRepo;
        private readonly IRepository<Feedback> _feedbackRepo;
        private readonly IPlatformCategoryService _platformCategoryService;
        private readonly IHostingEnvironment _env;
        private readonly IFeedbackService _feedbackService;

        public ManageController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IOptions<IdentityCookieOptions> identityCookieOptions,
          IEmailSender emailSender,
          ISmsSender smsSender,
          ILoggerFactory loggerFactory,
          IInfluencerRepository influencerRepo,
          IRepository<Platform> platformRepo,
          IRepository<Category> categoryRepo,
          IRepository<Feedback> feedbackRepo,
          IPlatformCategoryService platformCategoryService,
          IHostingEnvironment env, 
          IFeedbackService feedbackService)
        {
            _influencerRepo = influencerRepo;
            _platformRepo = platformRepo;
            _categoryRepo = categoryRepo;
            _feedbackRepo = feedbackRepo;
            _platformCategoryService = platformCategoryService;
            _env = env;
            _userManager = userManager;
            _signInManager = signInManager;
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<ManageController>();
            _feedbackService = feedbackService;
        }

        //
        // GET: /Manage/Index
        [HttpGet]
        public async Task<IActionResult> Profile(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

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

            var model = new IndexViewModel
            {
                //HasPassword = await _userManager.HasPasswordAsync(user),
                //PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                //TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
                //Logins = await _userManager.GetLoginsAsync(user),
                //BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
                ApplicationUser = user,
                Age = age,
                Gender = gender,
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel account)
        //{
        //    ManageMessageId? message = ManageMessageId.Error;
        //    var user = await GetCurrentUserAsync();
        //    if (user != null)
        //    {
        //        var result = await _userManager.RemoveLoginAsync(user, account.LoginProvider, account.ProviderKey);
        //        if (result.Succeeded)
        //        {
        //            await _signInManager.SignInAsync(user, isPersistent: false);
        //            message = ManageMessageId.RemoveLoginSuccess;
        //        }
        //    }
        //    return RedirectToAction(nameof(ManageLogins), new { Message = message });
        //}

        //
        // GET: /Manage/AddPhoneNumber
        //public IActionResult AddPhoneNumber()
        //{
        //    return View();
        //}

        ////
        //// POST: /Manage/AddPhoneNumber
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    // Generate the token and send it
        //    var user = await GetCurrentUserAsync();
        //    if (user == null)
        //    {
        //        return View("Error");
        //    }
        //    var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
        //    await _smsSender.SendSmsAsync(model.PhoneNumber, "Your security code is: " + code);
        //    return RedirectToAction(nameof(VerifyPhoneNumber), new { PhoneNumber = model.PhoneNumber });
        //}

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EnableTwoFactorAuthentication()
        //{
        //    var user = await GetCurrentUserAsync();
        //    if (user != null)
        //    {
        //        await _userManager.SetTwoFactorEnabledAsync(user, true);
        //        await _signInManager.SignInAsync(user, isPersistent: false);
        //        _logger.LogInformation(1, "User enabled two-factor authentication.");
        //    }
        //    return RedirectToAction(nameof(Index), "Manage");
        //}

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DisableTwoFactorAuthentication()
        //{
        //    var user = await GetCurrentUserAsync();
        //    if (user != null)
        //    {
        //        await _userManager.SetTwoFactorEnabledAsync(user, false);
        //        await _signInManager.SignInAsync(user, isPersistent: false);
        //        _logger.LogInformation(2, "User disabled two-factor authentication.");
        //    }
        //    return RedirectToAction(nameof(Index), "Manage");
        //}

        //
        // GET: /Manage/VerifyPhoneNumber
        //[HttpGet]
        //public async Task<IActionResult> VerifyPhoneNumber(string phoneNumber)
        //{
        //    var user = await GetCurrentUserAsync();
        //    if (user == null)
        //    {
        //        return View("Error");
        //    }
        //    var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
        //    // Send an SMS to verify the phone number
        //    return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        //}

        //
        // POST: /Manage/VerifyPhoneNumber
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    var user = await GetCurrentUserAsync();
        //    if (user != null)
        //    {
        //        var result = await _userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.Code);
        //        if (result.Succeeded)
        //        {
        //            await _signInManager.SignInAsync(user, isPersistent: false);
        //            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.AddPhoneSuccess });
        //        }
        //    }
        //    // If we got this far, something failed, redisplay the form
        //    ModelState.AddModelError(string.Empty, "Failed to verify phone number");
        //    return View(model);
        //}

        //
        // POST: /Manage/RemovePhoneNumber
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> RemovePhoneNumber()
        //{
        //    var user = await GetCurrentUserAsync();
        //    if (user != null)
        //    {
        //        var result = await _userManager.SetPhoneNumberAsync(user, null);
        //        if (result.Succeeded)
        //        {
        //            await _signInManager.SignInAsync(user, isPersistent: false);
        //            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.RemovePhoneSuccess });
        //        }
        //    }
        //    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        //}

        //
        // GET: /Manage/ChangePassword

        [HttpGet]
        [Route("/[controller]/Change/[action]")]
        public IActionResult Password()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/[controller]/Change/[action]")]
        public async Task<IActionResult> Password(ChangePasswordViewModel model)
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
                    _logger.LogInformation(3, "User changed their password successfully.");
                    TempData["Success"] = "Du har skiftet dit kodeord!";
                    return RedirectToAction(nameof(Profile), new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction(nameof(Profile), new { Message = ManageMessageId.Error });
        }

        //
        // GET: /Manage/SetPassword
        //[HttpGet]
        //public IActionResult SetPassword()
        //{
        //    return View();
        //}

        //
        // POST: /Manage/SetPassword
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var user = await GetCurrentUserAsync();
        //    if (user != null)
        //    {
        //        var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
        //        if (result.Succeeded)
        //        {
        //            await _signInManager.SignInAsync(user, isPersistent: false);
        //            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.SetPasswordSuccess });
        //        }
        //        AddErrors(result);
        //        return View(model);
        //    }
        //    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        //}

        //GET: /Manage/ManageLogins

        [HttpGet]
        public async Task<IActionResult> ManageLogins(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.AddLoginSuccess ? "The external login was added."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await _userManager.GetLoginsAsync(user);
            var otherLogins = _signInManager.GetExternalAuthenticationSchemes().Where(auth => userLogins.All(ul => auth.AuthenticationScheme != ul.LoginProvider)).ToList();
            ViewData["ShowRemoveButton"] = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkLogin(string provider)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.Authentication.SignOutAsync(_externalCookieScheme);

            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Action(nameof(LinkLoginCallback), "Manage");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return Challenge(properties, provider);
        }

        //
        // GET: /Manage/LinkLoginCallback
        [HttpGet]
        public async Task<ActionResult> LinkLoginCallback()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync(await _userManager.GetUserIdAsync(user));
            if (info == null)
            {
                return RedirectToAction(nameof(ManageLogins), new { Message = ManageMessageId.Error });
            }
            var result = await _userManager.AddLoginAsync(user, info);
            var message = ManageMessageId.Error;
            if (result.Succeeded)
            {
                message = ManageMessageId.AddLoginSuccess;
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.Authentication.SignOutAsync(_externalCookieScheme);
            }
            return RedirectToAction(nameof(ManageLogins), new { Message = message });
        }

        [HttpGet]
        [Route("/[controller]/Profile/[action]")]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            var influencer = _influencerRepo.Get(user.Id);

            var model = new EditProfileViewModel
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
        public async Task<IActionResult> Edit(EditProfileViewModel model, IFormFile profilePic)
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
            var influencer = _influencerRepo.Get(user.Id);
            InfluencerViewModel model;
            if (influencer != null)
            {
                model = new InfluencerViewModel()
                {
                    ApplicationUser = user,
                    Influencer = influencer,
                    ProfileText = influencer.ProfileText,
                };

                PopulatePlatforms(influencer.InfluenterPlatform, model);
            }
            else
            {
                model = new InfluencerViewModel()
                {
                    ApplicationUser = user,
                    Influencer = null
                };
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Influencer(InfluencerViewModel model, string[] categoriList, string[] link)
        {
            if (string.IsNullOrEmpty(model.Influencer.Alias))
            {
                TempData["Error"] = "Du skal udfylde dit Alias";
                return RedirectToAction("Influencer");
            }

            if (model.Influencer.Id == null)
            {
                var newInfluencer = new Influencer();
                newInfluencer.Id = model.ApplicationUser.Id;
                newInfluencer.Alias = model.Influencer.Alias;
                _influencerRepo.Add(newInfluencer);
            }

            var influencer = _influencerRepo.Get(model.ApplicationUser.Id);

            // Adding to categories

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
            UpdatePlatform(model.SecoundYoutubeLink, "SecondYouTube", influencer, platforms);
            UpdatePlatform(model.TwitterLink, "Twitter", influencer, platforms);
            UpdatePlatform(model.TwitchLink, "Twitch", influencer, platforms);
            UpdatePlatform(model.WebsiteLink, "Website", influencer, platforms);
            UpdatePlatform(model.SnapchatLink, "SnapChat", influencer, platforms);

            influencer.ProfileText = model.ProfileText;
            influencer.Alias = model.Influencer.Alias;

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

            var model = new FeedbackResponseViewModel()
            {
                Rating = rating
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AnswerFeedback(FeedbackResponseViewModel model)
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

            var model = new MinAnmeldelseViewModel()
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

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            AddLoginSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error,
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

        #endregion

    }
}
