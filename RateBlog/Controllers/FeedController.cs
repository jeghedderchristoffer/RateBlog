using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Controllers
{
    public class FeedController : Controller
    {
        public IActionResult Index()
        {
            return View(); 
        }
    }
}
