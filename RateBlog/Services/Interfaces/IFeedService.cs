using Bestfluence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Services.Interfaces
{
    public interface IFeedService
    {
        Task<IEnumerable<RSSFeed>> GetBlogFeedAsync(string blogUrl, string parameter);
        Task<IEnumerable<RSSFeed>> GetYoutubeFeedAsync(string youtubeUrl, string parameter);
        Task<IEnumerable<RSSFeed>> GetInstagramFeedAsync(string instagramUrl); 
        string GetTimeString(int hours);
    }
}
