using System.Collections.Generic;
using RateBlog.Models;

namespace RateBlog.Repository
{
    public interface IAdminRepository
    {
        void Add(ApplicationUser user);
        ApplicationUser Get(string Id);
        void EditUser(ApplicationUser user);
        List<ApplicationUser> GetAll();
        void Delete(string Id);

    }
}