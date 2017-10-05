using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections.Generic;
using Bestfluence.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Bestfluence.Controllers
{
    public class FeedController : Controller
    {

        [HttpPost]
        public async Task<IActionResult> Index()
        {

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("https://ceciliademant.dk");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage RSSdata = await client.GetAsync("/blog/feed");

                //string RSSData = client.DownloadString(RSSURL);
                if (RSSdata.IsSuccessStatusCode)
                {
                    XDocument xml = XDocument.Parse(RSSdata.ToString());
                    var RSSFeedData = (from x in xml.Descendants("item")
                                       select new RSSFeed
                                       {
                                           Title = ((string)x.Element("title")),
                                           Link = ((string)x.Element("link")),
                                           Description = ((string)x.Element("description")),
                                           PubData = ((string)x.Element("pubDate"))
                                       });
                    ViewBag.RSSFeed = RSSFeedData;               
                    
                }
                return View();
            }
                
          
        }
    }
}
