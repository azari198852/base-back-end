using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class ProductColor
    {
        public ProductColor()
        {
            ProductColorStatusLog = new HashSet<ProductColorStatusLog>();
        }

        public long Id { get; set; }
        public long? ProductId { get; set; }
        public long? ColorId { get; set; }
        public long? Price { get; set; }
        public long? ProducePrice { get; set; }
        public int? ProduceDuration { get; set; }
        public int? Count { get; set; }
        public long? Coding { get; set; }
        public long? FinalStatusId { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual Color Color { get; set; }
        public virtual Status FinalStatus { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<ProductColorStatusLog> ProductColorStatusLog { get; set; }
    }
}
