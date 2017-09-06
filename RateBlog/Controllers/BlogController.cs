using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RateBlog.Models;
using RateBlog.Models.BlogViewModel;
using RateBlog.Models.ManageViewModels;
using RateBlog.Repository;

namespace RateBlog.Controllers
{
    public class BlogController : Controller
    {
        private readonly IRepository<Blog> _blogRepo;
     
        public BlogController(IRepository<Blog> blogRepository)
        {

            _blogRepo = blogRepository;
        
        }
      
      
         public IActionResult Index()
           {

            var getAllBlogs = _blogRepo.GetAll();

            var blog = new BlogViewModel()
            {

                BlogList = getAllBlogs.ToList(),
                
            };

            return View(blog);
        }

        [HttpGet]
        public IActionResult NewsPage(string id)
        {
            var getBlog = _blogRepo.Get(id);
            
            var model = new BlogViewModel()
            {
                Blog = getBlog,
            };

            return View(model); 
        }

        [HttpGet]
        public IActionResult BlogList(string id)
        {
            var getAllBlogs = _blogRepo.GetAll();

            var blog = new BlogViewModel()
            {

                BlogList = getAllBlogs.ToList(),

            };

            return View(blog);
        }


        [HttpGet]
        public IActionResult UploadPictureView(string id)
        {
            var getBlog = _blogRepo.Get(id);

            var model = new BlogViewModel()
            {
                Blog = getBlog,
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult UploadPictureView(string id, BlogViewModel model)
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
       

        [HttpPost]
        public IActionResult DeleteBlog(string Id)
        {
            var getfeedback = _blogRepo.Get(Id);
            _blogRepo.Delete(getfeedback);
            return RedirectToAction("Index");
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
