using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class ProductCustomerRateImage
    {
        public long Id { get; set; }
        public long? ProductCustomerRateId { get; set; }
        public long? FileType { get; set; }
        public string FileUrl { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual ProductCustomerRate ProductCustomerRate { get; set; }
    }
}
