using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Bestfluence.Data;

namespace Bestfluence.Models
{
    public class Country : BaseEntity
    {
        [MaxLength(2)]
        public string Name { get; set; }
    }
}
