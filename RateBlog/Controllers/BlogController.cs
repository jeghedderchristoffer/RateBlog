using Microsoft.AspNetCore.Mvc;
using RateBlog.Models;
using RateBlog.Models.BlogViewModels;
using RateBlog.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Controllers
{
    public class BlogController : Controller
    {
        private readonly IRepository<BlogArticle> _blogRepo;

        public BlogController(IRepository<BlogArticle> blogRepository)
        {
            _blogRepo = blogRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var articles = _blogRepo.GetAll().Where(x => x.Publish == true).OrderByDescending(x => x.DateTime); 
            var blog = new IndexViewModel()
            {
                ArticleList = articles
            };
            return View(blog);
        }

        [HttpGet]
        public IActionResult Article(string id)
        {
            var article = _blogRepo.Get(id); 
            return View(article); 
        }

        [HttpGet]
        public IActionResult GetBlogPicture(string id, bool indexPic)  
        {
            var blog = _blogRepo.Get(id);
            byte[] buffer;

            if (indexPic)
            {
                buffer = blog.IndexPicture;
                return File(buffer, "image/jpg", string.Format("{0}.jpg", blog.IndexPicture));
            }
            else
            {
                buffer = blog.ArticlePicture;
                return File(buffer, "image/jpg", string.Format("{0}.jpg", blog.ArticlePicture));
            }
        }

    }
}
