using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using RateBlog.Data;
using RateBlog.Models;
using RateBlog.Models.InfluenterViewModels;
using RateBlog.Models.ManageViewModels;
using RateBlog.Repository;
using RateBlog.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RateBlog.Controllers
{
    public class InfluencerController : Controller
    {
        private readonly IInfluenterRepository _influenter;
        private readonly IRatingRepository _ratingRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPlatformRepository _platform;
        private readonly IKategoriRepository _kategori;

        public InfluencerController(IKategoriRepository kategori, IInfluenterRepository influenter, IRatingRepository ratingRepository, UserManager<ApplicationUser> userManager, IPlatformRepository platform)
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

            var model = new Models.InfluenterViewModels.IndexViewModel()
            {
                SearchString = search
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
        [Authorize]
        public IActionResult Create()
        {
            var model = new CreateViewModel()
            {
                IKList = GetInfluenterKategoriList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Opret en influencer
                var newInfluenter = new Influenter();
                newInfluenter.Alias = model.Influenter.Alias;
                _influenter.Add(newInfluenter);

                _platform.Insert(newInfluenter.InfluenterId, _platform.GetAll().SingleOrDefault(x => x.PlatformNavn == "YouTube").PlatformId, model.YoutubeLink);
                _platform.Insert(newInfluenter.InfluenterId, _platform.GetAll().SingleOrDefault(x => x.PlatformNavn == "Facebook").PlatformId, model.FacebookLink);
                _platform.Insert(newInfluenter.InfluenterId, _platform.GetAll().SingleOrDefault(x => x.PlatformNavn == "Instagram").PlatformId, model.InstagramLink);
                _platform.Insert(newInfluenter.InfluenterId, _platform.GetAll().SingleOrDefault(x => x.PlatformNavn == "SnapChat").PlatformId, model.SnapchatLink);
                _platform.Insert(newInfluenter.InfluenterId, _platform.GetAll().SingleOrDefault(x => x.PlatformNavn == "Twitter").PlatformId, model.TwitterLink);
                _platform.Insert(newInfluenter.InfluenterId, _platform.GetAll().SingleOrDefault(x => x.PlatformNavn == "Website").PlatformId, model.WebsiteLink);
                _platform.Insert(newInfluenter.InfluenterId, _platform.GetAll().SingleOrDefault(x => x.PlatformNavn == "Twitch").PlatformId, model.TwitchLink);

                foreach (var v in model.IKList)
                {
                    _kategori.Insert(newInfluenter.InfluenterId, _kategori.GetAll().SingleOrDefault(x => x.KategoriNavn == v.KategoriNavn).KategoriId, v.IsSelected);
                }

                // Lav en email baseret på deres alias... Hvis deres alias indeholde æøå vil den nok fejle...
                var email = newInfluenter.InfluenterId + "@" + newInfluenter.InfluenterId + ".dk";

                var user = new ApplicationUser();

                if (string.IsNullOrEmpty(model.Name))
                {
                    user.UserName = email;
                    user.Email = email;
                    user.Name = model.Influenter.Alias;

                    // Match applicationUser med influencer
                    user.InfluenterId = newInfluenter.InfluenterId;
                }
                else
                {
                    user.UserName = email;
                    user.Email = email;
                    user.Name = model.Name;

                    // Match applicationUser med influencer
                    user.InfluenterId = newInfluenter.InfluenterId;
                }

                // Billede af influencer...
                if (model.ProfilePic != null)
                {
                    MemoryStream ms = new MemoryStream();
                    model.ProfilePic.OpenReadStream().CopyTo(ms);
                    user.ImageFile = ms.ToArray();
                }

                // Koden vil være: bestfluence + InfluenterId + 123
                var result = await _userManager.CreateAsync(user, "bestfluence" + model.Influenter.InfluenterId + "123");

                if (result.Succeeded)
                {
                    TempData["Success"] = "Du har oprette denne influencer!"; 
                    return RedirectToAction("Show", new { id = newInfluenter.InfluenterId });
                }
                else
                {
                    // Der findes allrede en bruger med denne email, ergo findes denne influenter, da emailen er baseret på alias...
                    // ELLER så består alias af æøå, og dette må emailen ikke være....
                    // Jeg sletter derfor influenteren igen!! 
                    _influenter.Delete(newInfluenter.InfluenterId);
                    TempData["Error"] = "Der skete en fejl!";
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public PartialViewResult Sorter(int[] platforme, int[] kategorier, int pageIndex, int pageSize, string search)
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
        public PartialViewResult GetNextFromList(int pageIndex, int pageSize, string search, int[] platforme, int[] kategorier, string lastUser)
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

            // List: Keeps track of index, skip the previous and takes the next.
            var list = new List<ApplicationUser>();
            if ((platforme.Count() != 0 || kategorier.Count() != 0) && lastUser != null)
            {
                var last = _userManager.Users.FirstOrDefault(x => x.Id == lastUser); 
                var index = influenter.IndexOf(last) + 1;
                list = influenter.Skip(index).Take(pageSize).ToList();
            }
            else
            {
                list = influenter.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }

            // If platform or kategori is checked, this makes sure the the next 5 (pageSize) has that kategori or platform. 
            var sortList = _influenter.SortInfluencerByPlatAndKat(platforme, kategorier, list);




            return PartialView("InfluencerListPartial", sortList);
        }


        private List<InfluenterKategoriViewModel> GetInfluenterKategoriList()
        {
            return new List<InfluenterKategoriViewModel>()
            {
                new InfluenterKategoriViewModel(){ KategoriNavn = "DIY", IsSelected = false},
                new InfluenterKategoriViewModel(){ KategoriNavn = "Beauty", IsSelected = false},
                new InfluenterKategoriViewModel(){ KategoriNavn = "Entertainment", IsSelected = false},
                new InfluenterKategoriViewModel(){ KategoriNavn = "Fashion", IsSelected = false},
                new InfluenterKategoriViewModel(){ KategoriNavn = "Food", IsSelected = false},
                new InfluenterKategoriViewModel(){ KategoriNavn = "Gaming", IsSelected = false},
                new InfluenterKategoriViewModel(){ KategoriNavn = "Lifestyle", IsSelected = false },
                new InfluenterKategoriViewModel(){ KategoriNavn = "Mommy", IsSelected = false },
                new InfluenterKategoriViewModel(){ KategoriNavn = "VLOG", IsSelected = false },
            };
        }
    }
}

