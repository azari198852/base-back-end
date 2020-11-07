using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class CustomerOrder
    {
        public CustomerOrder()
        {
            CustomerOrderPayment = new HashSet<CustomerOrderPayment>();
            CustomerOrderProduct = new HashSet<CustomerOrderProduct>();
            CustomerOrderStatusLog = new HashSet<CustomerOrderStatusLog>();
        }

        public long Id { get; set; }
        public long? CustomerId { get; set; }
        public long? CustomerAddressId { get; set; }
        public long? OrderDate { get; set; }
        public long? OrderNo { get; set; }
        public long? FinalStatusId { get; set; }
        public long? OrderType { get; set; }
        public long? OrderProduceTime { get; set; }
        public long? PostTypeId { get; set; }
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
        public long? PackingPrice { get; set; }
        public long? PackingWeight { get; set; }
        public long? OrderWeight { get; set; }
        public long? FinalWeight { get; set; }
        public string CustomerDescription { get; set; }
        public string AdminDescription { get; set; }
        public string SellerDescription { get; set; }
        public long? SendDate { get; set; }
        public long? DeliveryDate { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual CustomerAddress CustomerAddress { get; set; }
        public virtual Status FinalStatus { get; set; }
        public virtual PaymentType PaymentType { get; set; }
        public virtual PostType PostType { get; set; }
        public virtual ICollection<CustomerOrderPayment> CustomerOrderPayment { get; set; }
        public virtual ICollection<CustomerOrderProduct> CustomerOrderProduct { get; set; }
        public virtual ICollection<CustomerOrderStatusLog> CustomerOrderStatusLog { get; set; }
    }
}
