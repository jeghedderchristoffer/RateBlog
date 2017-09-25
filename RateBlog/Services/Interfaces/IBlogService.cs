using Bestfluence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Services.Interfaces
{
    public interface IBlogService
    {
        IEnumerable<BlogArticle> GetLast4BlogArticle(); 
    }
}
