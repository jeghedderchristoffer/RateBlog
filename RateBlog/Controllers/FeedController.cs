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

namespace Bestfluence.Controllers
{
    public class FeedController : Controller
    {
        private readonly IFeedService _feedSevice;

        public FeedController(IFeedService feedService)
        {
            _feedSevice = feedService; 
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var rssFeedData = new List<RSSFeed>();

            rssFeedData.AddRange(await _feedSevice.GetBlogFeedAsync("http://ceciliademant.dk", "feed"));
            rssFeedData.AddRange(await _feedSevice.GetYoutubeFeedAsync("https://www.youtube.com/user/NordicBeautySecretsD", "/feeds/videos.xml?"));
            rssFeedData.AddRange(await _feedSevice.GetInstagramFeedAsync("cecilia.demant"));

            rssFeedData.AddRange(await _feedSevice.GetBlogFeedAsync("http://fielaursen.dk", "feed"));
            rssFeedData.AddRange(await _feedSevice.GetYoutubeFeedAsync("https://www.youtube.com/channel/UCAn3KlW4Ug1K-BGLMDEBNKA", "/feeds/videos.xml?"));
            rssFeedData.AddRange(await _feedSevice.GetInstagramFeedAsync("fielaursenofficial"));

            return View(rssFeedData); 
        }
    }
}
