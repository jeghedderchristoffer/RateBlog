using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using RateBlog.Data;
using RateBlog.Models;
using RateBlog.Models.InfluenterViewModels;
using RateBlog.Repository;
using RateBlog.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Controllers
{
    public class InfluenterController : Controller
    {
        private readonly IInfluenterRepository _influenter;
        private readonly IRatingRepository _ratingRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPlatformRepository _platform;
        private readonly IKategoriRepository _kategori;


        public InfluenterController(IKategoriRepository kategori, IInfluenterRepository influenter, IRatingRepository ratingRepository, UserManager<ApplicationUser> userManager, IPlatformRepository platform)
        {
            _influenter = influenter;
            _userManager = userManager;
            _ratingRepository = ratingRepository;
            _platform = platform;
            _kategori = kategori;
        }

        [HttpGet]
        public IActionResult Index(string search)
        {
            Dictionary<int, double> influenterRating = new Dictionary<int, double>();

            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }

            var influenter = _userManager.Users.
                Where(x => (x.Name.ToLower().Contains(search.ToLower())) && x.InfluenterId.HasValue
                || (x.Influenter.Alias.Contains(search) && x.InfluenterId.HasValue)).ToList();

            foreach (var kategori in _kategori.GetAll())
            {
                if (search.ToLower().Equals(kategori.KategoriNavn.ToLower()))
                {
                    influenter.AddRange(_kategori.GetAllInfluentersWithKategori(search));
                }
            }

            foreach (var platform in _platform.GetAll())
            {
                if (search.ToLower().Equals(platform.PlatformNavn.ToLower()))
                {
                    influenter.AddRange(_platform.GetAllInfluentersWithPlatform(search));
                }
            }

            foreach (var v in influenter)
            {
                influenterRating.Add(v.InfluenterId.Value, _ratingRepository.GetRatingAverage(v.InfluenterId.Value));
            } 

            var model = new IndexViewModel()
            {
                SearchString = search,
                InfluentList = influenter.Take(5).ToList(),
                InfluenterRatings = influenterRating
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Show(int id)
        {
            var influenter = _influenter.Get(id);

            //Burde kun kunne få den pågældene user, da Index() metoden KUN returnere Users som er influenter...
            var user = _userManager.Users.SingleOrDefault(x => x.InfluenterId == id);

            var model = new ShowViewModel()
            {
                ApplicationUser = user,
                Influenter = influenter
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Read(int id)
        {
            var influenter = _influenter.Get(id);
            var user = _userManager.Users.SingleOrDefault(x => x.InfluenterId == id);

            var model = new ReadViewModel()
            {
                ApplicationUser = user,
                Influenter = influenter
            };

            return View(model);
        }

        [HttpGet]
        public PartialViewResult Sorter(string[] currentUsers, int[] platforme, int[] kategorier)
        {
            // Get all users in the current search...
            var listOfUsers = new List<ApplicationUser>(); 
            foreach(var v in currentUsers)
            {
                listOfUsers.Add(_userManager.Users.FirstOrDefault(x => x.Id == v)); 
            }

            // Gets the selected platforms
            var sortList = _influenter.SortInfluencerByPlatAndKat(platforme, kategorier, listOfUsers);

            return PartialView("InfluencerListPartial", sortList); 
        }

        [HttpGet]
        public PartialViewResult GetNextFromList(int pageIndex, int pageSize, string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }

            var influenter = _userManager.Users.
                Where(x => (x.Name.ToLower().Contains(search.ToLower())) && x.InfluenterId.HasValue
                || (x.Influenter.Alias.Contains(search) && x.InfluenterId.HasValue)).ToList();

            foreach (var kategori in _kategori.GetAll())
            {
                if (search.ToLower().Equals(kategori.KategoriNavn.ToLower()))
                {
                    influenter.AddRange(_kategori.GetAllInfluentersWithKategori(search));
                }
            }

            foreach (var platform in _platform.GetAll())
            {
                if (search.ToLower().Equals(platform.PlatformNavn.ToLower()))
                {
                    influenter.AddRange(_platform.GetAllInfluentersWithPlatform(search));
                }
            }

            var list = influenter.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            return PartialView("InfluencerListPartial", list); 
        }
    }
}
