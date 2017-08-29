using System;
using System.Collections.Generic;
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
                blog = getBlog,
            };

            return View(model); 
        }

      
        public IActionResult CreateArticle()
        {
            var model = new Blog(); 
           
            return View(model);
        }


        [HttpPost]
        public IActionResult CreateArticle(Blog model)
        {
            if (ModelState.IsValid)
            {
                _blogRepo.Add(model);

            }
            return RedirectToAction("Index");
            
        }

        [HttpPost]
        public IActionResult DeleteFeedback(string Id)
        {
            var getfeedback = _blogRepo.Get(Id);
            _blogRepo.Delete(getfeedback);
            return RedirectToAction("Index");
        }

        
    }
}
