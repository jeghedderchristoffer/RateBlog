using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RateBlog.Data;
using RateBlog.Models;

namespace RateBlog.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

      

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
            
        }
        public void EditUser(ApplicationUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        } 
        public void Add(ApplicationUser user)
        {
            _context.ApplicationUser.Add(user);
            _context.SaveChanges();
        }
        //public void Delete(int Id)
        //{
        //    EkspertRating ekspertrating = _context.EkspertRating.Find(Id);
        //    _applicationDbContext.EkspertRating.Remove(ekspertrating);
        //    _applicationDbContext.SaveChanges();
        //}

        public ApplicationUser Get(string Id)
        {
            return _context.ApplicationUser.FirstOrDefault(x => x.Id == Id);
        }

        public List<ApplicationUser> GetAll()
        {
            return _context.ApplicationUser.ToList();
        }

    }
}
