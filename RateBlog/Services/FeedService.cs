using Bestfluence.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bestfluence.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Xml.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Bestfluence.Services
{
    public class FeedService : IFeedService
    {
        public string GetTimeString(int hours)
        {
            if (hours < 24)
            {
                if (hours == 0)
                    return "Nyt";
                if (hours == 1)
                    return hours + " time";

                return hours + " timer";
            }

            TimeSpan days = TimeSpan.FromHours(hours);
            var daysCount = (int)days.TotalDays;
            var result = (int)days.TotalDays + " dage";

            if (daysCount == 1)
            {
                result = daysCount + " dag";
            }

            return result;
        }

        public async Task<IEnumerable<RSSFeed>> GetBlogFeedAsync(string blogUrl, string parameter, string influencerId, string alias)
        {
            List<RSSFeed> RSSFeedData = new List<RSSFeed>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://" + blogUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                    HttpResponseMessage RSSdata = await client.GetAsync(parameter);

                    if (RSSdata.IsSuccessStatusCode && RSSdata.Content.Headers.ContentType.MediaType == "application/rss+xml")
                    {

                        XDocument xml = XDocument.Parse(await RSSdata.Content.ReadAsStringAsync());
                        XNamespace content = "http://purl.org/rss/1.0/modules/content/";
                        RSSFeedData = (from x in xml.Descendants("item")
                                       select new RSSFeed
                                       {
                                           Title = WebUtility.HtmlDecode(((string)x.Element("title"))),
                                           Link = WebUtility.HtmlDecode(((string)x.Element("link"))),
                                           Description = WebUtility.HtmlDecode(((string)x.Element("description"))),
                                           PubData = WebUtility.HtmlDecode(((string)x.Element("pubDate"))),
                                           Src = WebUtility.HtmlDecode(((string)x.Element(content + "encoded"))),
                                           Type = "Blog",
                                           Id = influencerId,
                                           Name = alias
                                       }).ToList();

                        var tempImgSrc = "";

                        foreach (var v in RSSFeedData)
                        {
                            v.Description = StripTagsCharArray(v.Description);

                            if (v.Description.Length > 250)
                                v.Description = v.Description.Substring(0, 250) + "...";

                            if (v.Src != null)
                            {
                                tempImgSrc = v.Src;
                                var pattern = new Regex("src=\"(.*?)jpe?g");
                                tempImgSrc = pattern.Match(v.Src).ToString().Replace('"', ' ');

                                if (string.IsNullOrWhiteSpace(tempImgSrc))
                                {
                                    pattern = new Regex("src=\"(.*?)\"");
                                    tempImgSrc = pattern.Match(v.Src).ToString().Replace('"', ' ');
                                }
                                v.Src = tempImgSrc;
                            }


                            var date = new DateTime();
                            var succeded = DateTime.TryParse(v.PubData, out date);

                            if (succeded)
                            {
                                v.TimeSincePublished = (int)Math.Floor((DateTime.Now - date).TotalHours);
                                v.TimeString = GetTimeString(v.TimeSincePublished);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }

            return RSSFeedData;
        }

        public async Task<IEnumerable<RSSFeed>> GetYoutubeFeedAsync(string youtubeUrl, string parameter, string influencerId, string alias)
        {
            IEnumerable<RSSFeed> RSSFeedData = new List<RSSFeed>();

            using (var client = new HttpClient())
            {
                if (youtubeUrl.Contains("channel"))
                {
                    youtubeUrl = youtubeUrl.Substring(youtubeUrl.IndexOf("channel") + 8);
                    youtubeUrl = "channel_id=" + youtubeUrl;
                }
                else
                {
                    youtubeUrl = youtubeUrl.Substring(youtubeUrl.IndexOf("user") + 5);
                    youtubeUrl = "user=" + youtubeUrl;
                }

                client.BaseAddress = new Uri("https://www.youtube.com");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/rss+xml"));
                HttpResponseMessage RSSdata = await client.GetAsync(parameter + youtubeUrl);

                if (RSSdata.IsSuccessStatusCode && RSSdata.Content.Headers.ContentType.MediaType == "text/xml")
                {
                    XDocument doc = XDocument.Parse(await RSSdata.Content.ReadAsStringAsync());
                    XNamespace xmlns = "http://www.w3.org/2005/Atom";
                    XNamespace media = "http://search.yahoo.com/mrss/";
                    XNamespace yt = "http://www.youtube.com/xml/schemas/2015";
                    RSSFeedData =
                        (from entry in doc.Root.Elements(xmlns + "entry")
                         let grp = entry.Element(media + "group")
                         select new RSSFeed
                         {
                             Title = (string)grp.Element(media + "title"),
                             Description = (string)grp.Element(media + "description"),
                             Src = "https://www.youtube.com/embed/" + (string)entry.Element(yt + "videoId"),
                             Link = "https://www.youtube.com/watch?v=" + (string)entry.Element(yt + "videoId"),
                             PubData = (string)entry.Element(xmlns + "published"),
                             Type = "Youtube",
                             Id = influencerId,
                             Name = alias
                         }).ToList();


                    foreach (var v in RSSFeedData)
                    {
                        if (v.Description.Length > 250)
                            v.Description = v.Description.Substring(0, 250) + "...";

                        var date = new DateTime();
                        var succeded = DateTime.TryParse(v.PubData, out date);

                        if (succeded)
                        {
                            v.TimeSincePublished = (int)Math.Floor((DateTime.Now - date).TotalHours);
                            v.TimeString = GetTimeString(v.TimeSincePublished);
                        }
                    }
                }
                return RSSFeedData;
            }
        }

        public async Task<IEnumerable<RSSFeed>> GetInstagramFeedAsync(string instagramUrl, string influencerId, string alias)
        {
            var url = "https://queryfeed.net/instagram?q=";

            List<RSSFeed> RSSFeedData = new List<RSSFeed>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage RSSdata = await client.GetAsync(instagramUrl);

                if (RSSdata.IsSuccessStatusCode)
                {

                    XDocument xml = XDocument.Parse(await RSSdata.Content.ReadAsStringAsync());
                    XNamespace media = "http://search.yahoo.com/mrss/";
                    RSSFeedData = (from x in xml.Descendants("item")
                                   select new RSSFeed
                                   {
                                       Link = "https://www.instagram.com/" + instagramUrl,
                                       Description = WebUtility.HtmlDecode(((string)x.Element("description"))),
                                       PubData = WebUtility.HtmlDecode(((string)x.Element("pubDate"))),
                                       Src = WebUtility.HtmlDecode(((string)x.Element(media + "thumbnail").Attribute("url"))),
                                       Type = "Instagram",
                                       Id = influencerId,
                                       Name = alias,
                                   }).ToList();

                    foreach (var v in RSSFeedData)
                    {
                        v.Description = StripTagsCharArray(v.Description) + " ";

                        if (v.Description.Length > 250)
                            v.Description = v.Description.Substring(0, 250) + "...";



                        var date = new DateTime();
                        var succeded = DateTime.TryParse(v.PubData, out date);

                        if (succeded)
                        {
                            v.TimeSincePublished = (int)Math.Floor((DateTime.Now - date).TotalHours);
                            v.TimeString = GetTimeString(v.TimeSincePublished);
                        }

                    }
                }
                return RSSFeedData;
            }
        }

        private string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }
}