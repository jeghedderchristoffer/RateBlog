using RateBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class InstagramApiData : BaseEntity
    {
        public string Id { get; set; }//fk appli-id

        public DateTime DataLastUpdated { get; set; }

        public string UserName { get; set; }

        public int Followers { get; set; }

        public int FollowedBy { get; set; }

        public int NumberImages { get; set; }


    }
}
