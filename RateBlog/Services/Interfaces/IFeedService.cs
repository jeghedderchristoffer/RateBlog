using Bestfluence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Services.Interfaces
{
    public interface IFeedService
    {
        Task<IEnumerable<RSSFeed>> GetBlogFeedAsync(string blogUrl, string parameter, string influencerId, string alias);
        Task<IEnumerable<RSSFeed>> GetYoutubeFeedAsync(string youtubeUrl, string parameter, string influencerId, string alias);
        Task<IEnumerable<RSSFeed>> GetInstagramFeedAsync(string instagramUrl, string influencerId, string alias); 
        string GetTimeString(int hours);
    }
}
