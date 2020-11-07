using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class PackingType
    {
        public PackingType()
        {
            CustomerOrderProduct = new HashSet<CustomerOrderProduct>();
            PackingTypeImage = new HashSet<PackingTypeImage>();
            ProductPackingType = new HashSet<ProductPackingType>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long? Price { get; set; }
        public long? Weight { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual ICollection<CustomerOrderProduct> CustomerOrderProduct { get; set; }
        public virtual ICollection<PackingTypeImage> PackingTypeImage { get; set; }
        public virtual ICollection<ProductPackingType> ProductPackingType { get; set; }
    }
}
