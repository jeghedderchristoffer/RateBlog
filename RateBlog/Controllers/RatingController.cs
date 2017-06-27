using Microsoft.AspNetCore.Mvc;
using RateBlog.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Controllers
{
    public class RatingController  : Controller
    {

        private IRatingRepository _rating;

        public RatingController(IRatingRepository rating)
        {
            _rating = rating;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RatingStars()
        {
            return View();
        }

    }

}

