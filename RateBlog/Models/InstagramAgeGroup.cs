using Bestfluence.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Bestfluence.Models
{
    public class InstagramAgeGroup : BaseEntity
    {
        public string InstagramDataId { get; set; }
        public virtual InstagramData InstagramData { get; set; }

        [DefaultValue(0)]
        public int Female13To17 { get; set; }

        [DefaultValue(0)]
        public int Female18To24 { get; set; }

        [DefaultValue(0)]
        public int Female25To34 { get; set; }

        [DefaultValue(0)]
        public int Female35To44 { get; set; }

        [DefaultValue(0)]
        public int Female45To55 { get; set; }

        [DefaultValue(0)]
        public int Female55To64 { get; set; }

        [DefaultValue(0)]
        public int Female65Plus { get; set; }

        [DefaultValue(0)]
        public int Male13To17 { get; set; }

        [DefaultValue(0)]
        public int Male18To24 { get; set; }

        [DefaultValue(0)]
        public int Male25To34 { get; set; }

        [DefaultValue(0)]
        public int Male35To44 { get; set; }

        [DefaultValue(0)]
        public int Male45To55 { get; set; }

        [DefaultValue(0)]
        public int Male55To64 { get; set; }

        [DefaultValue(0)]
        public int Male65Plus { get; set; }

    }
}
