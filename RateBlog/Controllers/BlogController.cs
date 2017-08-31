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
      
      
         public IActionResult Index(string searchString)
           {

            var getAllBlogs = _blogRepo.GetAll();

            var blog= new BlogViewModel()
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

      
        public IActionResult CreateArticle()
        {
            var model = new BlogViewModel(); 
           
            return View(model);
        }


        [HttpPost]
        public IActionResult CreateArticle(BlogViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.ArticlesPicture != null)
                {
                    MemoryStream ms = new MemoryStream();
                    model.ArticlesPicture.OpenReadStream().CopyTo(ms);
                    model.Blog.ArticlePicture = ms.ToArray();
                }

                if (model.IndexPicture != null)
                {
                    MemoryStream ms = new MemoryStream();
                    model.IndexPicture.OpenReadStream().CopyTo(ms);
                    model.Blog.IndexPicture = ms.ToArray();
                }

                _blogRepo.Add(model.Blog);

            }
            return RedirectToAction("Index");
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
