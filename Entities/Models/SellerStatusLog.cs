using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class SellerStatusLog
    {
        public long Id { get; set; }
        public long? SellerId { get; set; }
        public long? StatusId { get; set; }
        public long? ChangeDate { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual Seller Seller { get; set; }
        public virtual Status Status { get; set; }
    }
}
