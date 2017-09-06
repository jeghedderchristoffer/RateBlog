﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RateBlog.Models.AdminViewModels
{
    public class UploadPictureViewModel
    {
        public IFormFile ArticlesPicture { get; set; }
        public IFormFile IndexPicture { get; set; }
        public List<Blog> BlogList { get; set; }
    }
}
