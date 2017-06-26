using RateBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Repository
{
    public interface IInfluenterKategoriRepository
    {
        void Insert(int influenterId, int kategoriId, bool isSeleced);
        bool IsKategoriSelected(int influenterId, int kategoriId);
        List<string> GetAllKategori(int influenterId); 
    }
}
