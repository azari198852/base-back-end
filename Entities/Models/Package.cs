using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Package
    {
        public Package()
        {
            PackageImage = new HashSet<PackageImage>();
            PackageProduct = new HashSet<PackageProduct>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long? Price { get; set; }
        public long? StartDateTime { get; set; }
        public long? EndDateTime { get; set; }
        public string PackageImageUrl { get; set; }
        public int? ProductCount { get; set; }
        public long? ProductsPriceSum { get; set; }
        public int? Count { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual ICollection<PackageImage> PackageImage { get; set; }
        public virtual ICollection<PackageProduct> PackageProduct { get; set; }
    }
}
