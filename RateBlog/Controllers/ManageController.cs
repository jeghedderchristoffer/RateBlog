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


        private readonly IRepository<Influencer> _influencerRepo;
        private readonly IRepository<Platform> _platformRepo;
        private readonly IRepository<Category> _categoryRepo;
        private readonly IRepository<Feedback> _feedbackRepo;
        private readonly IPlatformCategoryService _platformCategoryService; 
        private readonly IHostingEnvironment _env; 

        public ManageController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IOptions<IdentityCookieOptions> identityCookieOptions,
          IEmailSender emailSender,
          ISmsSender smsSender,
          ILoggerFactory loggerFactory,
          IRepository<Influencer> influencerRepo,
          IRepository<Platform> platformRepo,
          IRepository<Category> categoryRepo,
          IRepository<Feedback> feedbackRepo, 
          IPlatformCategoryService platformCategoryService,
          IHostingEnvironment env)
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
            var model = new IndexViewModel
            {
                HasPassword = await _userManager.HasPasswordAsync(user),
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
                Logins = await _userManager.GetLoginsAsync(user),
                BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user)
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

            EditProfileViewModel model;

            if (influencer != null)
            {
                model = new EditProfileViewModel
                {
                    Name = user.Name,
                    Email = user.Email,
                    Year = user.Year,
                    Postnummer = user.Postnummer,
                    PhoneNumber = user.PhoneNumber,
                    ProfileText = user.ProfileText,
                    Gender = user.Gender,
                    Influenter = _influencerRepo.Get(user.Id),
                    YoutubeLink = _platformCategoryService.GetPlatformLink(influencer.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "YouTube").Id),
                    FacebookLink = _platformCategoryService.GetPlatformLink(influencer.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "Facebook").Id),
                    InstagramLink = _platformCategoryService.GetPlatformLink(influencer.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "Instagram").Id),
                    SnapchatLink = _platformCategoryService.GetPlatformLink(influencer.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "SnapChat").Id),
                    TwitterLink = _platformCategoryService.GetPlatformLink(influencer.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "Twitter").Id),
                    WebsiteLink = _platformCategoryService.GetPlatformLink(influencer.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "Website").Id),
                    TwitchLink = _platformCategoryService.GetPlatformLink(influencer.Id, _platformRepo.GetAll().SingleOrDefault(x => x.Name == "Twitch").Id),
                    IKList = await GetInfluenterKategoriList()
                };
            }
            else
            {
                model = new EditProfileViewModel
                {
                    Name = user.Name,
                    Email = user.Email,
                    Year = user.Year,
                    Postnummer = user.Postnummer,
                    PhoneNumber = user.PhoneNumber,
                    ProfileText = user.ProfileText,
                    Gender = user.Gender,
                    IKList = await GetInfluenterKategoriList()
                };
            }
            return View(model);
        }

            [HttpPost] 
        [Route("/[controller]/Profile/[action]")]
        public async Task<IActionResult> Edit(EditProfileViewModel model, IFormFile profilePic)
        {
            var user = await _userManager.GetUserAsync(User);

            if (model.IKList == null)
            {
                model.IKList = await GetInfluenterKategoriList();
            }

            if (ModelState.IsValid)
            {
                user.Email = model.Email;
                user.UserName = model.Email;
                user.Name = model.Name;
                user.Year = model.Year;
                user.Postnummer = model.Postnummer;
                user.PhoneNumber = model.PhoneNumber;
                user.ProfileText = model.ProfileText;


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

        [HttpPost]
        public async Task<IActionResult> EditInfluenterProfile(EditProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var influencer = _influencerRepo.Get(user.Id); 

            if (ModelState.IsValid)
            {
                user.Email = model.Email;
                user.UserName = model.Email;
                user.Name = model.Name;
                user.Year = model.Year;
                user.Postnummer = model.Postnummer;
                user.PhoneNumber = model.PhoneNumber;
                user.ProfileText = model.ProfileText;

                if (model.ProfilePic != null)
                {
                    MemoryStream ms = new MemoryStream();
                    model.ProfilePic.OpenReadStream().CopyTo(ms);
                    user.ProfilePicture = ms.ToArray();
                }

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded != true)
                {
                    TempData["Error"] = "Der findes allerede en bruger med denne email!";
                    return RedirectToAction("Edit", model);
                }

                // Add if influenterId is null
                if (influencer == null)
                {
                    var newInfluenter = new Influencer();
                    newInfluenter.Id = user.Id; 
                    newInfluenter.Alias = model.Influenter.Alias;
                    _influencerRepo.Add(newInfluenter);
                    influencer = newInfluenter; 
                }
                else
                {
                    influencer.Alias = model.Influenter.Alias;
                    _influencerRepo.Update(influencer);
                }

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

                if (result.Succeeded)
                {
                    TempData["Success"] = "Din profil blev opdateret!";
                    return RedirectToAction("Edit");
                }

                TempData["Error"] = "Der findes allerede en bruger med denne email!";
                return RedirectToAction("Edit", model);
            }

            if (ModelState["ProfilePic"] != null)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                var message = allErrors.First();
                TempData["Error"] = message.ErrorMessage;
            }
            else
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                var message = allErrors.First();
                TempData["Error"] = message.ErrorMessage;
            }


            return RedirectToAction("Edit", model);
        }

        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true, Duration = 0)]
        public IActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        [Route("/[controller]/Feedback/[action]/{id}")]
        public IActionResult Answer(string id)
        {
            var rating = _feedbackRepo.Get(id); 
            rating.IsRead = true;
            _feedbackRepo.Update(rating);

            var model = new FeedbackResponseViewModel()
            {
                Rating = rating
            };

            return View(model);
        }
        
        public IActionResult AnswerFeedback(FeedbackResponseViewModel model)
        {
            var rating = _feedbackRepo.Get(model.Rating.Id);
            rating.Answer = model.Rating.Answer;

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

        [HttpPost]
        [Route("/[controller]/Feedback/[action]/{id}")]
        public IActionResult Read(string id)
        {
            var rating = _feedbackRepo.Get(id);

            if (!string.IsNullOrEmpty(rating.Answer))
            {
                rating.IsAnswerRead = true;
                _feedbackRepo.Update(rating);
            }

            var model = new MinAnmeldelseViewModel()
            {
                Rating = rating
            };

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ProfilePic()
        {
            var user = await GetCurrentUserAsync();
            byte[] buffer = user.ProfilePicture;

            if(buffer == null)
            {
                var dir = _env.WebRootPath;
                var path = Path.Combine(dir, "/images", "BF" + ".png");
                return File(path, "image/jpeg"); 
            }

            return File(buffer, "image/jpg", string.Format("{0}.jpg", user.ProfilePicture));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult UsersProfilePic(string id)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == id);

            byte[] buffer = user.ProfilePicture;

            if (buffer == null)
            {
                var dir = _env.WebRootPath;
                var path = Path.Combine(dir, "/images", "BF" + ".png");
                return File(path, "image/jpeg");
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
            Error
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private async Task<List<InfluenterKategoriViewModel>> GetInfluenterKategoriList()
        {
            var user = await _userManager.GetUserAsync(User);
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
                return new List<InfluenterKategoriViewModel>()
                    {
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Lifestyle", IsSelected = false },
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Beauty", IsSelected = false },
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Entertainment", IsSelected = false },
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Fashion", IsSelected = false },
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Interests", IsSelected = false },
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Gaming", IsSelected = false},
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Personal", IsSelected = false },
                    };
            }



        }

        #endregion

    }
}
