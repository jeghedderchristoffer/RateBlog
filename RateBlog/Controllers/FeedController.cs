using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bestfluence.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Text.RegularExpressions;
using Bestfluence.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Bestfluence.Models.FeedViewModels;
using Bestfluence.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bestfluence.Controllers
{
    public class FeedController : Controller
    {
        private readonly IFeedService _feedSevice;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public FeedController(IFeedService feedService, ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _feedSevice = feedService;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            user = await _dbContext.Users.Include(x => x.UserFollowers).ThenInclude(x => x.Influencer).ThenInclude(x => x.InfluenterPlatform).ThenInclude(x => x.Platform).SingleOrDefaultAsync(x => x.Id == user.Id);
            var model = new IndexViewModel()
            {
                InfluencerList = user.UserFollowers
            };
            return View(model); 
        }

        [HttpGet]
        public async Task<JsonResult> GetFeed(int index)
        {
            var user = await _userManager.GetUserAsync(User);
            user = await _dbContext.Users.Include(x => x.UserFollowers).ThenInclude(x => x.Influencer).ThenInclude(x => x.InfluenterPlatform).ThenInclude(x => x.Platform).SingleOrDefaultAsync(x => x.Id == user.Id);

            var list = new List<RSSFeed>();

            foreach (var v in user.UserFollowers)
            {
                foreach(var p in v.Influencer.InfluenterPlatform)
                {
                    if(p.Platform.Name == "Website")
                    {
                        list.AddRange(await _feedSevice.GetBlogFeedAsync(p.Link, "feed", v.InfluencerId, v.Influencer.Alias.ToUpper()));
                    }
                    else if (p.Platform.Name == "YouTube" || p.Platform.Name == "SecondYouTube")
                    {
                        list.AddRange(await _feedSevice.GetYoutubeFeedAsync(p.Link, "/feeds/videos.xml?", v.InfluencerId, v.Influencer.Alias.ToUpper()));
                    }
                    //else if (p.Platform.Name == "Instagram")
                    //{
                    //    list.AddRange(await _feedSevice.GetInstagramFeedAsync(p.Link, v.InfluencerId, v.Influencer.Alias.ToUpper()));
                    //}
                }
            }

            return Json(list.OrderBy(x => x.TimeSincePublished).Skip(index * 20).Take(20)); 
        }
    }
}
