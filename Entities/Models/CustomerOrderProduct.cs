using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class CustomerOrderProduct
    {
        public CustomerOrderProduct()
        {
            CustomerOrderProductStatusLog = new HashSet<CustomerOrderProductStatusLog>();
        }

        public long Id { get; set; }
        public long? PackingTypeId { get; set; }
        public long? CustomerOrderId { get; set; }
        public long? ProductId { get; set; }
        public long? SellerId { get; set; }
        public string ProductName { get; set; }
        public long? ProductPrice { get; set; }
        public long? ProductIncreasePrice { get; set; }
        public long? FinalStatusId { get; set; }
        public long? OrderType { get; set; }
        public long? ProductOfferId { get; set; }
        public double? ProductOfferValue { get; set; }
        public string ProductOfferCode { get; set; }
        public long? ProductOfferPrice { get; set; }
        public int? OrderCount { get; set; }
        public long? Weight { get; set; }
        public long? PackingPrice { get; set; }
        public long? PackingWeight { get; set; }
        public long? ProductCode { get; set; }
        public long? FinalWeight { get; set; }
        public string Description { get; set; }
        public string CustomerDescription { get; set; }
        public string SellerDescription { get; set; }
        public string AdminDescription { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual CustomerOrder CustomerOrder { get; set; }
        public virtual Status FinalStatus { get; set; }
        public virtual PackingType PackingType { get; set; }
        public virtual Product Product { get; set; }
        public virtual Seller Seller { get; set; }
        public virtual ICollection<CustomerOrderProductStatusLog> CustomerOrderProductStatusLog { get; set; }
    }
}
