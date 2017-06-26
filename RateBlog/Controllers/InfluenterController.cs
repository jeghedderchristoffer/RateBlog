﻿using Microsoft.AspNetCore.Mvc;
using RateBlog.Data;
using RateBlog.Models.InfluenterViewModels;
using RateBlog.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Controllers
{
    public class InfluenterController : Controller
    {

        private IInfluenterRepository _influenter;

        public InfluenterController(IInfluenterRepository influenter)
        { 
            _influenter = influenter;
        }


        public IActionResult Index(string search)
        {

            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }
            //var influenter = _influenter.GetAll().FindAll(x => x.Name.ToLower().Contains(search.ToLower()));


            var model = new IndexViewModel()
            {
                SearchString = search               
            };

            return View(model); 
        }







    }
}
