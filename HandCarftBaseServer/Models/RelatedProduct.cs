using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class RelatedProduct
    {
        public long Id { get; set; }
        public long? OriginProductId { get; set; }
        public long? DestinProductId { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual Product DestinProduct { get; set; }
        public virtual Product OriginProduct { get; set; }
    }
}
