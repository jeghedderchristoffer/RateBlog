using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RateBlog.Models;
using RateBlog.Data;
using Microsoft.EntityFrameworkCore;

namespace RateBlog.Repository
{
    public class InfluencerRepository : IInfluencerRepository
    {
        private readonly ApplicationDbContext _context;

        public InfluencerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Influencer entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Influencer.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Influencer entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Influencer.Remove(entity);
            _context.SaveChanges();
        }

        public Influencer Get(string id)
        {
            return _context.Influencer
                .Include(x => x.InfluenterKategori)
                    .ThenInclude(x => x.Category)
                .Include(x => x.InfluenterPlatform)
                    .ThenInclude(x => x.Platform)
                .Include(x => x.Ratings)
                    .ThenInclude(x => x.ApplicationUser)
                .SingleOrDefault(x => x.Id == id); 
        }

        public IEnumerable<Influencer> GetAll()
        {
            return _context.Influencer
                .Include(x => x.InfluenterKategori)
                    .ThenInclude(x => x.Category)
                .Include(x => x.InfluenterPlatform)
                    .ThenInclude(x => x.Platform)
                .Include(x => x.Ratings);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(Influencer entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Influencer.Update(entity);
            _context.SaveChanges();
        }
    }
}
