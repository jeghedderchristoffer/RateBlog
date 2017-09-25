using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bestfluence.Data;
using Bestfluence.Models;
using Bestfluence.Models.BlogViewModels;
using Bestfluence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Controllers
{
    public class BlogController : Controller
    {
        private readonly IRepository<BlogArticle> _blogRepo;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager; 

        public BlogController(IRepository<BlogArticle> blogRepository, ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _blogRepo = blogRepository;
            _dbContext = dbContext;
            _userManager = userManager; 
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
        public async Task<IActionResult> Article(string id, string elementToScroll)
        {
            BlogArticle article; 
            if(_dbContext.BlogArticles.Any(x => x.Url == id))
            {
                article = await _dbContext.BlogArticles.Include(x => x.BlogRatings).Include(x => x.BlogComments).ThenInclude(x => x.ApplicationUser).SingleOrDefaultAsync(x => x.Url == id);
            }
            else
            {
                article = await _dbContext.BlogArticles.Include(x => x.BlogRatings).Include(x => x.BlogComments).ThenInclude(x => x.ApplicationUser).SingleOrDefaultAsync(x => x.Id == id);
            }

            var user = await _userManager.GetUserAsync(User);

            var model = new ArticleViewModel();

            if(user != null)
            {
                if (article.BlogRatings.Any(x => x.ApplicationUserId == user.Id))
                    model.HasVoted = true;
                else
                    model.HasVoted = false;
            }
            

            model.Article = article;
            model.ElementToScroll = elementToScroll; 


            return View(model); 
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateComment(string comment, string id)
        {
            var article = await _dbContext.BlogArticles.Include(x => x.BlogComments).SingleOrDefaultAsync(x => x.Id == id); 

            if (!string.IsNullOrEmpty(comment))
            {
                var user = await _userManager.GetUserAsync(User);

                article.BlogComments.Add(new BlogComment() { ApplicationUserId = user.Id, DateTime = DateTime.Now, BlogArticleId = id, Comment = comment });
                await _dbContext.SaveChangesAsync();
            }

            if (string.IsNullOrEmpty(article.Url))
            {
                return RedirectToAction("Article", new { id = id, elementToScroll = "blog-comment-container" });
            }
            else
            {
                return RedirectToRoute("blog", new { id = article.Url, elementToScroll = "blog-comment-container" });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RateArticle(string id, int rating)
        {
            var article = await _dbContext.BlogArticles.Include(x => x.BlogRatings).SingleOrDefaultAsync(x => x.Id == id);
            var user = await _userManager.GetUserAsync(User);
            article.BlogRatings.Add(new BlogRating() { BlogArticleId = article.Id, ApplicationUserId = user.Id, Rate = rating });
            await _dbContext.SaveChangesAsync();

            if (string.IsNullOrEmpty(article.Url))
            {
                return RedirectToAction("Article", new { id = id, elementToScroll = "blograting" });
            }
            else
            {
                return RedirectToRoute("blog", new { id = article.Url, elementToScroll = "blograting" });
            }
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
