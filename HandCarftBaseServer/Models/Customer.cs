using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerAddress = new HashSet<CustomerAddress>();
            CustomerOffer = new HashSet<CustomerOffer>();
            CustomerOrder = new HashSet<CustomerOrder>();
            CustomerStatusLog = new HashSet<CustomerStatusLog>();
            InversePresenterCustomer = new HashSet<Customer>();
            ProductCustomerRate = new HashSet<ProductCustomerRate>();
        }

        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? PresenterCustomerId { get; set; }
        public long? CustomerCode { get; set; }
        public string Name { get; set; }
        public string Fname { get; set; }
        public long? MelliCode { get; set; }
        public long? Mobile { get; set; }
        public long? Bdate { get; set; }
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }
        public string ProfileImageHurl { get; set; }
        public long? LocationId { get; set; }
        public long? FinalStatusId { get; set; }
        public bool? SendNews { get; set; }
        public bool? HaveMobileApp { get; set; }
        public string MobileAppVersion { get; set; }
        public long? MobileAppTypeId { get; set; }
        public long? CustomerClubCode { get; set; }
        public long? WalletFinalPrice { get; set; }
        public long? CustomerFinalScore { get; set; }
        public long? WorkId { get; set; }
        public long? PresentationCode { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual Status FinalStatus { get; set; }
        public virtual Location Location { get; set; }
        public virtual MobileAppType MobileAppType { get; set; }
        public virtual Customer PresenterCustomer { get; set; }
        public virtual Users User { get; set; }
        public virtual Work Work { get; set; }
        public virtual ICollection<CustomerAddress> CustomerAddress { get; set; }
        public virtual ICollection<CustomerOffer> CustomerOffer { get; set; }
        public virtual ICollection<CustomerOrder> CustomerOrder { get; set; }
        public virtual ICollection<CustomerStatusLog> CustomerStatusLog { get; set; }
        public virtual ICollection<Customer> InversePresenterCustomer { get; set; }
        public virtual ICollection<ProductCustomerRate> ProductCustomerRate { get; set; }
    }
}
