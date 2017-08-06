using System.Collections.Generic;
using RateBlog.Models;

namespace RateBlog.Repository
{
    public interface IAdminRepository
    {
        void Add(ApplicationUser user);
        ApplicationUser Get(string Id);
        List<ApplicationUser> GetAll();
    }
}