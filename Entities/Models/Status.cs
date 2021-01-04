using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Status
    {
        public Status()
        {
            Customer = new HashSet<Customer>();
            CustomerOrder = new HashSet<CustomerOrder>();
            CustomerOrderPayment = new HashSet<CustomerOrderPayment>();
            CustomerOrderProduct = new HashSet<CustomerOrderProduct>();
            CustomerOrderProductStatusLog = new HashSet<CustomerOrderProductStatusLog>();
            CustomerOrderStatusLog = new HashSet<CustomerOrderStatusLog>();
            CustomerStatusLog = new HashSet<CustomerStatusLog>();
            InverseNextStatus = new HashSet<Status>();
            Product = new HashSet<Product>();
            ProductColor = new HashSet<ProductColor>();
            ProductColorStatusLog = new HashSet<ProductColorStatusLog>();
            ProductStatusLog = new HashSet<ProductStatusLog>();
            Seller = new HashSet<Seller>();
            SellerDocument = new HashSet<SellerDocument>();
            SellerStatusLog = new HashSet<SellerStatusLog>();
            SellerCatProduct = new HashSet<SellerCatProduct>();
        }

        public long Id { get; set; }
        public long? CatStatusId { get; set; }
        public long? StatusTypeId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public long? NextStatusId { get; set; }
        public string Description { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual CatStatus CatStatus { get; set; }
        public virtual Status NextStatus { get; set; }
        public virtual StatusType StatusType { get; set; }
        public virtual ICollection<Customer> Customer { get; set; }
        public virtual ICollection<CustomerOrder> CustomerOrder { get; set; }
        public virtual ICollection<CustomerOrderPayment> CustomerOrderPayment { get; set; }
        public virtual ICollection<CustomerOrderProduct> CustomerOrderProduct { get; set; }
        public virtual ICollection<CustomerOrderProductStatusLog> CustomerOrderProductStatusLog { get; set; }
        public virtual ICollection<CustomerOrderStatusLog> CustomerOrderStatusLog { get; set; }
        public virtual ICollection<CustomerStatusLog> CustomerStatusLog { get; set; }
        public virtual ICollection<Status> InverseNextStatus { get; set; }
        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<ProductColor> ProductColor { get; set; }
        public virtual ICollection<ProductColorStatusLog> ProductColorStatusLog { get; set; }
        public virtual ICollection<ProductStatusLog> ProductStatusLog { get; set; }
        public virtual ICollection<Seller> Seller { get; set; }
        public virtual ICollection<SellerDocument> SellerDocument { get; set; }
        public virtual ICollection<SellerStatusLog> SellerStatusLog { get; set; }
        public virtual ICollection<SellerCatProduct> SellerCatProduct { get; set; }
    }
}
