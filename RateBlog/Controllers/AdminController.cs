using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RateBlog.Data;
using RateBlog.Models;
using RateBlog.Models.AdminViewModels;
using RateBlog.Repository;


namespace RateBlog.Controllers
{

    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IRepository<Influencer> _influenter;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Feedback> _feedBack;
        private readonly IRepository<Blog> _blogRepo;

        public AdminController(UserManager<ApplicationUser> userManager, IRepository<Influencer> influenter, IRepository<Feedback> feedBack, IRepository<Blog> blogRepository)
        {
            _feedBack = feedBack;
            _influenter = influenter;
            _userManager = userManager;
            _blogRepo = blogRepository;
        }
  //public IndexViewModel viewmodel = new IndexViewModel();

        public IActionResult Index(string searchString)
        {
            var model = _userManager.Users;

            if (!String.IsNullOrEmpty(searchString))
            {
                model = _userManager.Users.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
            }
            else
            {
                model = _userManager.Users;
            }
            viewmodel.Influencer = model.ToList();

            viewmodel.InfluentList = model.ToList();

            return View(viewmodel);
        //Virkede ikke så revertede. 
        //var model = IndexViewModel()

      
        }

        //redigere brugeren side
        public IActionResult EditUser(string id)
        {

            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);

            var model = new SeMereViewModel()
            {
                ApplicationUser = user,
            };

            return View(model);

        }
        //Skal bruges senere.
        public IActionResult EditInfluencer()
        {

            return View();
        }

        [HttpGet]
        public IActionResult ProfilePage(string id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);

            var model = new SeMereViewModel()
            {
                ApplicationUser = user,
            };

            return View(model);
        }


        //Dette er et testview, kan slettes når det lystets, husk at slette tilhørende view.
        [HttpGet]
        public IActionResult SeMere(string id)
        {

            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
           // var user = _userManager.Users.SingleOrDefault(x => x.Id == vmodel.ApplicationUser.Id);        
            var GetAllRating2 = _feedBack.GetAll();

            var rating = new SeFeedbackViewModel()
            {

                ListRating = GetAllRating2.ToList(),
                ApplicationUser = user,

            };

            return View(rating);

        }

        [HttpGet]
        public IActionResult SeFeedback(string id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);


            var getAllRatings = _feedBack.GetAll();

            var rating = new SeFeedbackViewModel()
            {

                ListRating = getAllRatings.ToList(),
                ApplicationUser = user,
            };

            return View(rating);

        }

        [HttpGet]
        public IActionResult RedigereFeedback(string Id)
        {
            var getRating = _feedBack.Get(Id);

            var getUserName = _userManager.Users.SingleOrDefault(x => x.Id == getRating.ApplicationUserId).Name;



            var rating = new SeFeedbackViewModel()
            {
                feedBack = getRating,
                AnmelderNavn = getUserName

            };

            return View(rating);
        }
       

        //[HttpPost]
        //public IActionResult RedigereFeedback(SeFeedbackViewModel SeFeedBackModel)
        //{
        //    var getRating = _feedBack.Get(SeFeedBackModel.feedBack.Id);
        //    getRating.Kvalitet = SeFeedBackModel.feedBack.Kvalitet;
        //    getRating.Opførsel = SeFeedBackModel.feedBack.Opførsel;
        //    getRating.Interaktion = SeFeedBackModel.feedBack.Interaktion;
        //    getRating.Troværdighed = SeFeedBackModel.feedBack.Troværdighed;
        //    // getRating.Feedback = SeFeedBackModel.Rating.Feedback;
        //    getRating.Answer = SeFeedBackModel.feedBack.Answer;
        //    _feedBack.Update(getRating);
        //    var getrating = _feedBack.Get(SeFeedBackModel.feedBack.Id);

        //    var rating = new SeFeedbackViewModel()
        //    {
        //        feedBack = getrating
        //    };

        //    return View(rating);
        //}

        //[HttpPost]
        //public async Task<IActionResult> EditUser(SeMereViewModel vmodel)
        //{
        //    var getUser = _userManager.Users.SingleOrDefault(x => x.Id == vmodel.ApplicationUser.Id);
            
        //    getUser.Email = vmodel.ApplicationUser.Email;
        //    getUser.Name = vmodel.ApplicationUser.Name;


        //    //getUser.InfluenterId = vmodel.ApplicationUser.InfluenterId;
        //    getUser.ProfileText = vmodel.ApplicationUser.ProfileText;
        //    getUser.PhoneNumber = vmodel.ApplicationUser.PhoneNumber;
        //    getUser.PasswordHash = vmodel.ApplicationUser.PasswordHash;
        //    getUser.LockoutEnd = vmodel.ApplicationUser.LockoutEnd;
        //    getUser.Gender = vmodel.ApplicationUser.Gender;
            

        //    // _userManager.EditUser(getUser);
        //    var result = await _userManager.UpdateAsync(getUser);


        //    var model = new SeMereViewModel()
        //    {
        //        ApplicationUser = getUser,

        //    };

        //    return View(model);

        //}

        [HttpPost]
        public IActionResult DeleteFeedback(string Id)
        {
            var getfeedback = _feedBack.Get(Id);
            _feedBack.Delete(getfeedback);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteUser(string id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> BanUser(string id, int dage)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            user.LockoutEnd = DateTime.Now.AddDays(dage);
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["ErrorMessage"] = "Der skete en fejl";
                return View();
            }



        }

        public IActionResult CreateArticle()
        {
            var model = new CreateArticlesViewModel();

            return View(model);
        }

        [HttpPost]
        public JsonResult CreateArticle(CreateArticlesViewModel model)
        {
            if (ModelState.IsValid)
            {
                Blog blog = new Blog()
                {
                    ArticleHeader = model.ArticleHeader,
                    DateTime = model.DateTime,
                    Author = model.Author,
                    Categories = model.Categories,
                    BriefText = model.BriefText,
                    ArticleText = model.ArticleText,
                };

                _blogRepo.Add(blog);

                return Json(true);
            }

            return Json(false);
        }


        [HttpGet]
        public IActionResult BlogList(string id)
        {
            var getAllBlogs = _blogRepo.GetAll();

            var blog = new BlogListViewModel()
            {

                BlogList = getAllBlogs.ToList(),

            };

            return View(blog);
        }


        [HttpGet]
        public IActionResult UploadPictureView(string id)
        {
            var getBlog = _blogRepo.Get(id);

            var model = new UploadPictureViewMidel()
            {
                Blog = getBlog,
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult UploadPictureView(string id, UploadPictureViewMidel model)
        {
            var blog = _blogRepo.Get(id);


            if (ModelState.IsValid)
            {


                if (model.ArticlesPicture != null)
                {
                    MemoryStream ms = new MemoryStream();
                    model.ArticlesPicture.OpenReadStream().CopyTo(ms);
                    blog.ArticlePicture = ms.ToArray();
                }

                if (model.IndexPicture != null)
                {
                    MemoryStream ms = new MemoryStream();
                    model.IndexPicture.OpenReadStream().CopyTo(ms);
                    blog.IndexPicture = ms.ToArray();
                }

                _blogRepo.Update(blog);
            }
            return RedirectToAction("Index", model);

        }


        [HttpGet]
        public IActionResult BlogProfilePic(string id)
        {
            var blog = _blogRepo.Get(id);
            byte[] buffer = blog.ArticlePicture;
            return File(buffer, "image/jpg", string.Format("{0}.jpg", blog.ArticlePicture));
        }

        [HttpGet]
        public IActionResult IndexProfilePic(string id)
        {
            var blog = _blogRepo.Get(id);
            byte[] buffer = blog.IndexPicture;
            return File(buffer, "image/jpg", string.Format("{0}.jpg", blog.IndexPicture));
        }

    }
}

