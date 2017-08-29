using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models.BlogViewModel
{
    public class BlogViewModel
    {

        public string SearchString { get; set; }
        public List<Blog> BlogList { get; set; }
        public Blog blog { get; set; }
    }
}
