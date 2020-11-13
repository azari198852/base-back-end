using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Entities.DataTransferObjects
{
    public class CustomerOrderFullDto
    {
        public long Id { get; set; }
        public long? CustomerId { get; set; }
        public long? CustomerAddressId { get; set; }
        public string CustomerAddress { get; set; }
        public long? CustomerMobile { get; set; }
        public string CustomerName { get; set; }
        public string OrderDate { get; set; }
        public long? OrderNo { get; set; }
        public long? FinalStatusId { get; set; }
        public string FinalStatus { get; set; }
        public string PaymentStatus { get; set; }
        public long? OrderType { get; set; }
        public long? OrderProduceTime { get; set; }
        public long? PostTypeId { get; set; }
        public string PostTypeName { get; set; }
        public long? PostTypePrice { get; set; }
        public string PostTrackingCode { get; set; }
        public long? PostServicePrice { get; set; }
        public double? TaxValue { get; set; }
        public long? TaxPrice { get; set; }
        public long? OfferId { get; set; }
        public int? OfferValue { get; set; }
        public long? OfferPrice { get; set; }
        public long? OrderPrice { get; set; }
        public long? FinalPrice { get; set; }
        public long? PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }
        public long? PackingPrice { get; set; }
        public long? PackingWeight { get; set; }
        public long? OrderWeight { get; set; }
        public long? FinalWeight { get; set; }
        public string CustomerDescription { get; set; }
        public string AdminDescription { get; set; }
        public string SellerDescription { get; set; }
        public string SendDate { get; set; }
        public string DeliveryDate { get; set; }
        public List<CustomerOrderProductDto> CustomerOrderProductsList { get; set; }
        public List<CustomerOrderPaymentDto> CustomerOrderPaymentList { get; set; }

    }
}
