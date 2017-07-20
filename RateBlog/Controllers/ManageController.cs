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

        private readonly IInfluenterRepository _influenterRepo;
        private readonly IPlatformRepository _platformRepo;
        private readonly IKategoriRepository _kategoriRepo;
        private readonly IRatingRepository _ratingRepo;

        public ManageController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IOptions<IdentityCookieOptions> identityCookieOptions,
          IEmailSender emailSender,
          ISmsSender smsSender,
          ILoggerFactory loggerFactory,
          IInfluenterRepository influenterRepo,
          IPlatformRepository platformRepo,
          IKategoriRepository kategoriRepo,
          IRatingRepository ratingRepo)
        {
            _influenterRepo = influenterRepo;
            _platformRepo = platformRepo;
            _kategoriRepo = kategoriRepo;
            _ratingRepo = ratingRepo;

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
        public async Task<IActionResult> Index(ManageMessageId? message = null)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel account)
        {
            ManageMessageId? message = ManageMessageId.Error;
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.RemoveLoginAsync(user, account.LoginProvider, account.ProviderKey);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    message = ManageMessageId.RemoveLoginSuccess;
                }
            }
            return RedirectToAction(nameof(ManageLogins), new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public IActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
            await _smsSender.SendSmsAsync(model.PhoneNumber, "Your security code is: " + code);
            return RedirectToAction(nameof(VerifyPhoneNumber), new { PhoneNumber = model.PhoneNumber });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, true);
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(1, "User enabled two-factor authentication.");
            }
            return RedirectToAction(nameof(Index), "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, false);
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(2, "User disabled two-factor authentication.");
            }
            return RedirectToAction(nameof(Index), "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        [HttpGet]
        public async Task<IActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
            // Send an SMS to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.Code);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.AddPhoneSuccess });
                }
            }
            // If we got this far, something failed, redisplay the form
            ModelState.AddModelError(string.Empty, "Failed to verify phone number");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePhoneNumber()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.SetPhoneNumberAsync(user, null);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.RemovePhoneSuccess });
                }
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //
        // GET: /Manage/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
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
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //
        // GET: /Manage/SetPassword
        [HttpGet]
        public IActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

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
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            EditProfileViewModel model;

            if (user.InfluenterId.HasValue)
            {
                var influenter = _influenterRepo.Get(user.InfluenterId.Value);

                model = new EditProfileViewModel
                {
                    Name = user.Name,
                    Email = user.Email,
                    Birth = user.Birth,
                    City = user.City,
                    PhoneNumber = user.PhoneNumber,
                    ProfileText = user.ProfileText,
                    ProfilePicture = user.ImageFile,
                    Influenter = _influenterRepo.Get(user.InfluenterId.Value),
                    YoutubeLink = _platformRepo.GetLink(influenter.InfluenterId, _platformRepo.GetAll().SingleOrDefault(x => x.PlatformNavn == "YouTube").PlatformId),
                    FacebookLink = _platformRepo.GetLink(influenter.InfluenterId, _platformRepo.GetAll().SingleOrDefault(x => x.PlatformNavn == "Facebook").PlatformId),
                    InstagramLink = _platformRepo.GetLink(influenter.InfluenterId, _platformRepo.GetAll().SingleOrDefault(x => x.PlatformNavn == "Instagram").PlatformId),
                    SnapchatLink = _platformRepo.GetLink(influenter.InfluenterId, _platformRepo.GetAll().SingleOrDefault(x => x.PlatformNavn == "SnapChat").PlatformId),
                    TwitterLink = _platformRepo.GetLink(influenter.InfluenterId, _platformRepo.GetAll().SingleOrDefault(x => x.PlatformNavn == "Twitter").PlatformId),
                    WebsiteLink = _platformRepo.GetLink(influenter.InfluenterId, _platformRepo.GetAll().SingleOrDefault(x => x.PlatformNavn == "Website").PlatformId),
                    TwitchLink = _platformRepo.GetLink(influenter.InfluenterId, _platformRepo.GetAll().SingleOrDefault(x => x.PlatformNavn == "Twitch").PlatformId),

                    IKList = await GetInfluenterKategoriList()
                };
            }
            else
            {
                model = new EditProfileViewModel
                {
                    Name = user.Name,
                    Email = user.Email,
                    Birth = user.Birth,
                    City = user.City,
                    PhoneNumber = user.PhoneNumber,
                    ProfileText = user.ProfileText,
                    IKList = await GetInfluenterKategoriList()
                };
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model, IFormFile profilePic)
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
                user.Birth = model.Birth;
                user.City = model.City;
                user.PhoneNumber = model.PhoneNumber;
                user.ProfileText = model.ProfileText;

                if (profilePic != null)
                {
                    MemoryStream ms = new MemoryStream();
                    profilePic.OpenReadStream().CopyTo(ms);
                    user.ImageFile = ms.ToArray();
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

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditInfluenterProfile(EditProfileViewModel model, IFormFile profilePicInfluencer)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                user.Email = model.Email;
                user.UserName = model.Email;
                user.Name = model.Name;
                user.Birth = model.Birth;
                user.City = model.City;
                user.PhoneNumber = model.PhoneNumber;
                user.ProfileText = model.ProfileText;

                if (profilePicInfluencer != null)
                {
                    MemoryStream ms = new MemoryStream();
                    profilePicInfluencer.OpenReadStream().CopyTo(ms);
                    user.ImageFile = ms.ToArray();
                }

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded != true)
                {
                    TempData["Error"] = "Der findes allerede en bruger med denne email!";
                    return View("EditProfile", model);
                }

                // Add if influenterId is null
                if (user.InfluenterId == null)
                {
                    var newInfluenter = new Influenter();
                    newInfluenter.Alias = model.Influenter.Alias;
                    _influenterRepo.Add(newInfluenter);
                    user.InfluenterId = newInfluenter.InfluenterId;
                }

                var influenter = _influenterRepo.Get(user.InfluenterId.Value);
                influenter.Alias = model.Influenter.Alias;
                _influenterRepo.Update(influenter);

                // Indsætter links og platforme, hvis de ikke er null. Koden skal nok laves om...

                _platformRepo.Insert(influenter.InfluenterId, _platformRepo.GetAll().SingleOrDefault(x => x.PlatformNavn == "YouTube").PlatformId, model.YoutubeLink);
                _platformRepo.Insert(influenter.InfluenterId, _platformRepo.GetAll().SingleOrDefault(x => x.PlatformNavn == "Facebook").PlatformId, model.FacebookLink);
                _platformRepo.Insert(influenter.InfluenterId, _platformRepo.GetAll().SingleOrDefault(x => x.PlatformNavn == "Instagram").PlatformId, model.InstagramLink);
                _platformRepo.Insert(influenter.InfluenterId, _platformRepo.GetAll().SingleOrDefault(x => x.PlatformNavn == "SnapChat").PlatformId, model.SnapchatLink);
                _platformRepo.Insert(influenter.InfluenterId, _platformRepo.GetAll().SingleOrDefault(x => x.PlatformNavn == "Twitter").PlatformId, model.TwitterLink);
                _platformRepo.Insert(influenter.InfluenterId, _platformRepo.GetAll().SingleOrDefault(x => x.PlatformNavn == "Website").PlatformId, model.WebsiteLink);
                _platformRepo.Insert(influenter.InfluenterId, _platformRepo.GetAll().SingleOrDefault(x => x.PlatformNavn == "Twitch").PlatformId, model.TwitchLink);


                // Insætter kategori
                foreach (var v in model.IKList)
                {
                    _kategoriRepo.Insert(influenter.InfluenterId, _kategoriRepo.GetAll().SingleOrDefault(x => x.KategoriNavn == v.KategoriNavn).KategoriId, v.IsSelected);
                }

                if (result.Succeeded)
                {
                    TempData["Success"] = "Din profil blev opdateret!";
                    return RedirectToAction("EditProfile");
                }

                TempData["Error"] = "Der findes allerede en bruger med denne email!";
                return View("EditProfile", model);
            }

            TempData["Error"] = "Du skal udfylde dine informationer for at kunne blive influenter!";
            return View("EditProfile", model);
        }

        [HttpGet]
        public IActionResult MyFeedback()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FeedbackResponse(int id)
        {
            var rating = _ratingRepo.Get(id);
            rating.IsRead = true;
            _ratingRepo.Update(rating);

            var model = new FeedbackResponseViewModel()
            {
                Rating = rating
            };

            return View(model);
        }

        public IActionResult Answer(FeedbackResponseViewModel model)
        {
            var rating = _ratingRepo.Get(model.Rating.RatingId);
            rating.Answer = model.Rating.Answer;

            _ratingRepo.Update(rating);

            TempData["Success"] = "Du har sendt dit svar!";

            return RedirectToAction("MyFeedback");
        }

        [HttpGet]
        [AllowAnonymous]     
        public async Task<IActionResult> ProfilePic()
        {
            var user = await GetCurrentUserAsync();

            byte[] buffer = user.ImageFile;
            return File(buffer, "image/jpg", string.Format("{0}.jpg", user.ImageFile));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult UsersProfilePic(string id)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == id);
            byte[] buffer = user.ImageFile;
            return File(buffer, "image/jpg", string.Format("{0}.jpg", user.ImageFile));
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

            if (user.InfluenterId.HasValue)
            {
                var influenter = _influenterRepo.Get(user.InfluenterId.Value);

                return new List<InfluenterKategoriViewModel>()
                    {
                        new InfluenterKategoriViewModel(){ KategoriNavn = "DIY", IsSelected = _kategoriRepo.IsKategoriSelected(influenter.InfluenterId , _kategoriRepo.GetIdByName("DIY")) },
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Beauty", IsSelected = _kategoriRepo.IsKategoriSelected(influenter.InfluenterId, _kategoriRepo.GetIdByName("Beauty")) },
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Entertainment", IsSelected = _kategoriRepo.IsKategoriSelected(influenter.InfluenterId, _kategoriRepo.GetIdByName("Entertainment"))  },
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Fashion", IsSelected = _kategoriRepo.IsKategoriSelected(influenter.InfluenterId, _kategoriRepo.GetIdByName("Fashion")) },
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Food", IsSelected = _kategoriRepo.IsKategoriSelected(influenter.InfluenterId, _kategoriRepo.GetIdByName("Food"))},
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Gaming", IsSelected = _kategoriRepo.IsKategoriSelected(influenter.InfluenterId, _kategoriRepo.GetIdByName("Gaming"))},
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Lifestyle", IsSelected = _kategoriRepo.IsKategoriSelected(influenter.InfluenterId, _kategoriRepo.GetIdByName("Lifestyle")) },
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Mommy", IsSelected = _kategoriRepo.IsKategoriSelected(influenter.InfluenterId, _kategoriRepo.GetIdByName("Mommy")) },
                        new InfluenterKategoriViewModel(){ KategoriNavn = "VLOG", IsSelected = _kategoriRepo.IsKategoriSelected(influenter.InfluenterId, _kategoriRepo.GetIdByName("VLOG")) },
                    };
            }
            else
            {
                return new List<InfluenterKategoriViewModel>()
                    {
                        new InfluenterKategoriViewModel(){ KategoriNavn = "DIY", IsSelected = false },
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Beauty", IsSelected = false },
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Entertainment", IsSelected = false  },
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Fashion", IsSelected = false},
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Food", IsSelected = false},
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Gaming", IsSelected = false},
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Lifestyle", IsSelected = false },
                        new InfluenterKategoriViewModel(){ KategoriNavn = "Mommy", IsSelected = false },
                        new InfluenterKategoriViewModel(){ KategoriNavn = "VLOG", IsSelected = false },
                    };
            }



        }

        #endregion

    }
}
