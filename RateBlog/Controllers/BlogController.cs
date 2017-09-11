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

        [HttpPost]
        public IActionResult DeleteBlog(string Id)
        {
            var getfeedback = _blogRepo.Get(Id);
            _blogRepo.Delete(getfeedback);
            return RedirectToAction("Index");
        }

    }
}
