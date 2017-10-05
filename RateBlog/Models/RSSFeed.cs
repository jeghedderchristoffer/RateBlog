﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bestfluence.Data;

namespace Bestfluence.Models
{
    public class RSSFeed : BaseEntity
    {

        public string Link { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PubData { get; set; }
    }
}
