using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class SellerDocument
    {
        public long Id { get; set; }
        public long? SellerId { get; set; }
        public long? DocumentId { get; set; }
        public string FileUrl { get; set; }
        public long? FianlStatusId { get; set; }
        public string AdminDescription { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual Document Document { get; set; }
        public virtual Status FianlStatus { get; set; }
        public virtual Seller Seller { get; set; }
    }
}
