using RateBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Repository
{
    interface IPlatformRepository
    {
        Platform Get(int Id);

        List<Platform> GetAll();

  
    }
}
