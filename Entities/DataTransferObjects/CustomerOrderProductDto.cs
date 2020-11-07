using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class CustomerOrderProductDto
    {
        public long Id { get; set; }
        public long? PackingTypeId { get; set; }
        public long? CustomerOrderId { get; set; }
        public long? ProductId { get; set; }
        public long? SellerId { get; set; }
        public string SellerName { get; set; }
        public string PaymentStatus { get; set; }
        public string ProductName { get; set; }
        public long? ProductPrice { get; set; }
        public long? ProductIncreasePrice { get; set; }
        public long? FinalStatusId { get; set; }
        public string StatusName { get; set; }
        public long? OrderType { get; set; }
        public long? ProductOfferId { get; set; }
        public double? ProductOfferValue { get; set; }
        public string ProductOfferCode { get; set; }
        public long? ProductOfferPrice { get; set; }
        public int? OrderCount { get; set; }
        public long? Weight { get; set; }
        public string PackingTypeName { get; set; }
        public long? PackingPrice { get; set; }
        public long? PackingWeight { get; set; }
        public long? ProductCode { get; set; }
        public long? FinalWeight { get; set; }
        public string Description { get; set; }
        public string CustomerDescription { get; set; }
        public string SellerDescription { get; set; }
        public string AdminDescription { get; set; }
    }
}
