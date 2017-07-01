using RateBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Repository
{
    public interface IKategoriRepository
    {
        /// <summary>
        /// Get single category by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Kategori Get(int Id);

        /// <summary>
        /// Get all category objects
        /// </summary>
        /// <returns></returns>
        List<Kategori> GetAll();

        /// <summary>
        /// Get single categoryId by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int GetIdByName(string name);

        /// <summary>
        /// Get all category names for a specific influenter
        /// </summary>
        /// <param name="influenterId"></param>
        /// <returns></returns>
        List<string> GetInfluentersCategories(int influenterId);

        /// <summary>
        /// Insert a influenters choosen category to the database. If the category was choosen before, and not now, it removes the category.
        /// </summary>
        /// <param name="influenterId"></param>
        /// <param name="kategoriId"></param>
        /// <param name="isSelected"></param>
        void Insert(int influenterId, int kategoriId, bool isSelected);

        /// <summary>
        /// If the influenter has the category, return true. Otherwise return false.
        /// </summary>
        /// <param name="influenterId"></param>
        /// <param name="kategoriId"></param>
        /// <returns></returns>
        bool IsKategoriSelected(int influenterId, int kategoriId);

    }
}
