﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.InfluenterViewModels
{
    public class ReadViewModel
    {

        public Influenter Influenter { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<ApplicationUser> InfluentList { get; set; }
        public int MyProperty { get; set; }

    }
}