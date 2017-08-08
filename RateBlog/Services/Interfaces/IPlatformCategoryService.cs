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
        /// Sorts influencers by platform and category
        /// </summary>
        /// <param name="platformIds">Platform IDs</param>
        /// <param name="categoryIds">Category IDs</param>
        /// <param name="users">Current users</param>
        /// <returns></returns>
        IEnumerable<ApplicationUser> SortInfluencerByPlatAndCat(int[] platformIds, int[] categoryIds, List<ApplicationUser> users);

        /// <summary>
        /// Gets category id by its name
        /// </summary>
        /// <param name="name">Category Name</param>
        /// <returns></returns>
        int GetCategoryIdByName(string name);

        /// <summary>
        /// Gets the category names with are asociated with a specific influencer. 
        /// </summary>
        /// <param name="id">Influencer ID</param>
        /// <returns></returns>
        IEnumerable<string> GetInfluencerCategoryNames(int id);

        /// <summary>
        /// Inserts a category for a influencer. 
        /// </summary>
        /// <param name="influencerId">Influencer ID</param>
        /// <param name="categoryId">Category ID</param>
        /// <param name="isSelected">Does the current category exist?</param>
        void InsertCategory(int influencerId, int categoryId, bool isSelected);

        /// <summary>
        /// Does the influencer have this category?
        /// </summary>
        /// <param name="influencerId">Influencer ID</param>
        /// <param name="categoryId">Category ID</param>
        /// <returns></returns>
        bool IsCategorySelected(int influencerId, int categoryId);

        /// <summary>
        /// Inserts a platform for a influencer
        /// </summary>
        /// <param name="influencerId">Influencer ID</param>
        /// <param name="platformId">Platform ID</param>
        /// <param name="link">Link to platform</param>
        void InsertPlatform(int influencerId, int platformId, string link);

        /// <summary>
        /// Gets the link to the platform
        /// </summary>
        /// <param name="influencerId">Influencer ID</param>
        /// <param name="platformId">Platform ID</param>
        /// <returns></returns>
        string GetPlatformLink(int influencerId, int platformId);

        /// <summary>
        /// Gets all the InfluencerPlatform object for a influencer
        /// </summary>
        /// <param name="id">Influencer ID</param>
        /// <returns></returns>
        IEnumerable<InfluencerPlatform> GetAllInfluencerPlatformForInfluencer(int id);

        /// <summary>
        /// Gets platform id by its name
        /// </summary>
        /// <param name="name">Platform Name</param>
        /// <returns></returns>
        int GetPlatformIdByName(string name);

        /// <summary>
        /// Gets all influencers with this platform name
        /// </summary>
        /// <param name="name">Platform Name</param>
        /// <returns></returns>
        IEnumerable<ApplicationUser> GetAllInfluencersWithPlatform(string name);

        /// <summary>
        /// Gets all influencers with this category name
        /// </summary>
        /// <param name="name">Category Name</param>
        /// <returns></returns>
        IEnumerable<ApplicationUser> GetAllInfluencersWithCategory(string name); 
    }
}
