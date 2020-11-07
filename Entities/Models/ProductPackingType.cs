using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class ProductPackingType
    {
        public ProductPackingType()
        {
            ProductPackingTypeImage = new HashSet<ProductPackingTypeImage>();
        }

        public long Id { get; set; }
        public long? ProductId { get; set; }
        public long? PackinggTypeId { get; set; }
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

        public virtual PackingType PackinggType { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<ProductPackingTypeImage> ProductPackingTypeImage { get; set; }
    }
}
