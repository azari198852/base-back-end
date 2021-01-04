using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public partial class SellerCatProduct
    {
        public long Id { get; set; }
        public long? SellerId { get; set; }
        public long? CatProductId { get; set; }
        public long? FinalStatusId { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual CatProduct CatProduct { get; set; }
        public virtual Status FinalStatus { get; set; }
        public virtual Seller Seller { get; set; }
    }
}
