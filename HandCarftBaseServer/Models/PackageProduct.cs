using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class PackageProduct
    {
        public long Id { get; set; }
        public long? PackageId { get; set; }
        public long? ProductId { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual Package Package { get; set; }
        public virtual Product Product { get; set; }
    }
}
