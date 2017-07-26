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
        public PartialViewResult Sorter(string[] currentUsers, int[] platforme, int[] kategorier, int pageIndex, int pageSize, string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }

            // Get all influencer users who match the search string. 
            var influenter = _userManager.Users.
                Where(x => (x.Name.ToLower().Contains(search.ToLower())) && x.InfluenterId.HasValue
                || (x.Influenter.Alias.Contains(search) && x.InfluenterId.HasValue)).ToList();

            // Get all influencer if the search word is kategori
            foreach (var kategori in _kategori.GetAll())
            {
                if (search.ToLower().Equals(kategori.KategoriNavn.ToLower()))
                {
                    influenter.AddRange(_kategori.GetAllInfluentersWithKategori(search));
                }
            }

            // Get all influencer if the search word is platform
            foreach (var platform in _platform.GetAll())
            {
                if (search.ToLower().Equals(platform.PlatformNavn.ToLower()))
                {
                    influenter.AddRange(_platform.GetAllInfluentersWithPlatform(search));
                }
            }


            var list = influenter.Take(pageSize * pageIndex).ToList();

            // If platform or kategori is checked, this makes sure the the next 5 (pageSize) has that kategori or platform. 
            var sortList = _influenter.SortInfluencerByPlatAndKat(platforme, kategorier, list);

            // If you sort, but the current users dont have enough to return pageSize, loop through until you get 5 or at worst, return all (under pageSize)
            // Den burde gerne returnere pageSize + næste index. Så hvis pageSize med index 1 indeholder 7 med gaming, og næste indeholder 2
            // burde den returnere 9 i alt. Er dog ikke 100%...........
            var maxPageIndex = (influenter.Count / pageSize) + 1;
            for (int i = pageIndex; sortList.Count < pageSize && i <= maxPageIndex; i++)
            {
                list = influenter.Take(pageSize * i).ToList();
                sortList = _influenter.SortInfluencerByPlatAndKat(platforme, kategorier, list);
            }

            return PartialView("InfluencerListPartial", sortList);
        }

        [HttpGet]
        public PartialViewResult GetNextFromList(int pageIndex, int pageSize, string search, int[] platforme, int[] kategorier, string[] currentUsers)
        {
            // Set string to empty string
            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }

            // Get all influencer users who match the search string. 
            var influenter = _userManager.Users.
                Where(x => (x.Name.ToLower().Contains(search.ToLower())) && x.InfluenterId.HasValue
                || (x.Influenter.Alias.Contains(search) && x.InfluenterId.HasValue)).ToList();

            // Get all influencer if the search word is kategori
            foreach (var kategori in _kategori.GetAll())
            {
                if (search.ToLower().Equals(kategori.KategoriNavn.ToLower()))
                {
                    influenter.AddRange(_kategori.GetAllInfluentersWithKategori(search));
                }
            }

            // Get all influencer if the search word is platform
            foreach (var platform in _platform.GetAll())
            {
                if (search.ToLower().Equals(platform.PlatformNavn.ToLower()))
                {
                    influenter.AddRange(_platform.GetAllInfluentersWithPlatform(search));
                }
            }

            // Gets current users...
            var listOfUsers = new List<ApplicationUser>();
            foreach (var v in currentUsers)
            {
                listOfUsers.Add(_userManager.Users.FirstOrDefault(x => x.Id == v));
            }

            // List: Keeps track of index, skip the previous and takes the next.
            var list = new List<ApplicationUser>(); 
            if((platforme.Count() != 0 || kategorier.Count() != 0) && listOfUsers.Count != 0)
            {
                var lastUser = listOfUsers.Last();
                var index = influenter.IndexOf(lastUser) + 1;
                list = influenter.Skip(index).Take(pageSize).ToList(); 
            }
            else
            {
                list = influenter.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }                      

            // If platform or kategori is checked, this makes sure the the next 5 (pageSize) has that kategori or platform. 
            var sortList = _influenter.SortInfluencerByPlatAndKat(platforme, kategorier, list);
            var endList = new List<ApplicationUser>();

            // SKAL MÅSKE SENDE PAGEINDEX TILBAGE, FOR AT FORTÆLLE HVILKET SIDEN JEG STOPPEDE VED!!!!!!!!!
            foreach (var v in sortList)
            {
                if (!listOfUsers.Contains(v))
                {
                    endList.Add(v);
                }
            }

            return PartialView("InfluencerListPartial", endList);
        }
    }
}
