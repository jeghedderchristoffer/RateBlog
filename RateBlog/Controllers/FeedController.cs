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

namespace Bestfluence.Controllers
{
    public class FeedController : Controller
    {

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://fielaursen.dk");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage RSSdata = await client.GetAsync("/feed");

                if (RSSdata.IsSuccessStatusCode)
                {


                    XDocument xml = XDocument.Parse(await RSSdata.Content.ReadAsStringAsync());
                    var RSSFeedData = (from x in xml.Descendants("item")
                                       select new RSSFeed
                                       {
                                           Title = WebUtility.HtmlDecode(((string)x.Element("title"))),
                                           Link = WebUtility.HtmlDecode(((string)x.Element("link"))),
                                           Description = WebUtility.HtmlDecode(((string)x.Element("description"))),
                                           PubData = WebUtility.HtmlDecode(((string)x.Element("pubDate")))
                                       }).ToList();

                    foreach(var v in RSSFeedData)
                    {
                        if (v.Description.Length > 250)
                            v.Description = v.Description.Substring(0, 250) + "..."; 
                    }

                    return View(RSSFeedData);

                }
                return View();
            }
        }
    }
}
