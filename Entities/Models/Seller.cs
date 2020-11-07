using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Seller
    {
        public Seller()
        {
            CustomerOrderProduct = new HashSet<CustomerOrderProduct>();
            Product = new HashSet<Product>();
            SellerAddress = new HashSet<SellerAddress>();
            SellerStatusLog = new HashSet<SellerStatusLog>();
        }

        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? RealOrLegal { get; set; }
        public long? SellerCode { get; set; }
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
        public bool? HaveMobileApp { get; set; }
        public string MobileAppVersion { get; set; }
        public long? MobileAppTypeId { get; set; }
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
        public virtual Users User { get; set; }
        public virtual ICollection<CustomerOrderProduct> CustomerOrderProduct { get; set; }
        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<SellerAddress> SellerAddress { get; set; }
        public virtual ICollection<SellerStatusLog> SellerStatusLog { get; set; }
    }
}
