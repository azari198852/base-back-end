using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Offer
    {
        public Offer()
        {
            CustomerOffer = new HashSet<CustomerOffer>();
            ProductOffer = new HashSet<ProductOffer>();
        }

        public long Id { get; set; }
        public long? OfferTypeId { get; set; }
        public string Name { get; set; }
        public long? FromDate { get; set; }
        public long? ToDate { get; set; }
        public double? Value { get; set; }
        public string OfferCode { get; set; }
        public long? MaximumPrice { get; set; }
        public int? UsageCount { get; set; }
        public double? UsageValue { get; set; }
        public long? MaximumUsagePrice { get; set; }
        public int? UsedCount { get; set; }
        public string Description { get; set; }
        public bool? HaveTimer { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual OfferType OfferType { get; set; }
        public virtual ICollection<CustomerOffer> CustomerOffer { get; set; }
        public virtual ICollection<ProductOffer> ProductOffer { get; set; }
    }
}
