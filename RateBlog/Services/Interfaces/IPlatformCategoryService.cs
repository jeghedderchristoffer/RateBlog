using RateBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Services.Interfaces
{
    public interface IPlatformCategoryService
    {
        /// <summary>
        /// Gets category id by its name
        /// </summary>
        /// <param name="name">Category Name</param>
        /// <returns></returns>
        string GetCategoryIdByName(string name);

        /// <summary>
        /// Gets the category names with are asociated with a specific influencer. 
        /// </summary>
        /// <param name="id">Influencer ID</param>
        /// <returns></returns>
        IEnumerable<string> GetInfluencerCategoryNames(string id);

        /// <summary>
        /// Inserts a category for a influencer. 
        /// </summary>
        /// <param name="influencerId">Influencer ID</param>
        /// <param name="categoryId">Category ID</param>
        /// <param name="isSelected">Does the current category exist?</param>
        void InsertCategory(string influencerId, string categoryId, bool isSelected);

        /// <summary>
        /// Does the influencer have this category?
        /// </summary>
        /// <param name="influencerId">Influencer ID</param>
        /// <param name="categoryId">Category ID</param>
        /// <returns></returns>
        bool IsCategorySelected(string influencerId, string categoryId);

        /// <summary>
        /// Inserts a platform for a influencer
        /// </summary>
        /// <param name="influencerId">Influencer ID</param>
        /// <param name="platformId">Platform ID</param>
        /// <param name="link">Link to platform</param>
        void InsertPlatform(string influencerId, string platformId, string link);

        /// <summary>
        /// Gets the link to the platform
        /// </summary>
        /// <param name="influencerId">Influencer ID</param>
        /// <param name="platformId">Platform ID</param>
        /// <returns></returns>
        string GetPlatformLink(string influencerId, string platformId);

        /// <summary>
        /// Gets all the InfluencerPlatform object for a influencer
        /// </summary>
        /// <param name="id">Influencer ID</param>
        /// <returns></returns>
        IEnumerable<InfluencerPlatform> GetAllInfluencerPlatformForInfluencer(string id);

        /// <summary>
        /// Gets platform id by its name
        /// </summary>
        /// <param name="name">Platform Name</param>
        /// <returns></returns>
        string GetPlatformIdByName(string name);

        /// <summary>
        /// Gets all influencers with this platform name
        /// </summary>
        /// <param name="name">Platform Name</param>
        /// <returns></returns>
        Task<IEnumerable<ApplicationUser>> GetAllInfluencersWithPlatform(string name);

        /// <summary>
        /// Gets all influencers with this category name
        /// </summary>
        /// <param name="name">Category Name</param>
        /// <returns></returns>
        Task<IEnumerable<ApplicationUser>> GetAllInfluencersWithCategory(string name);

    }
}
