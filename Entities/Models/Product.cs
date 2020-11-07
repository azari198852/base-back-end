using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Entities.Models
{
    public partial class Product
    {
        public Product()
        {
            CustomerOrderProduct = new HashSet<CustomerOrderProduct>();
            PackageProduct = new HashSet<PackageProduct>();
            ProductCatProductParameters = new HashSet<ProductCatProductParameters>();
            ProductColor = new HashSet<ProductColor>();
            ProductCustomerRate = new HashSet<ProductCustomerRate>();
            ProductImage = new HashSet<ProductImage>();
            ProductLanguage = new HashSet<ProductLanguage>();
            ProductOffer = new HashSet<ProductOffer>();
            ProductPackingType = new HashSet<ProductPackingType>();
            ProductStatusLog = new HashSet<ProductStatusLog>();
            RelatedProductDestinProduct = new HashSet<RelatedProduct>();
            RelatedProductOriginProduct = new HashSet<RelatedProduct>();
        }

        public long Id { get; set; }
        public long? CatProductId { get; set; }
        public long? SellerId { get; set; }
        public long? ProductMeterId { get; set; }
        public string Name { get; set; }
        public string EnName { get; set; }
        public long? Rkey { get; set; }
        public long? Coding { get; set; }
        public long? Price { get; set; }
        public long? ProducePrice { get; set; }
        public long? FinalStatusId { get; set; }
        public long? FirstCount { get; set; }
        public long? Count { get; set; }
        public string CoverImageUrl { get; set; }
        public string CoverImageHurl { get; set; }
        public long? SeenCount { get; set; }
        public long? LastSeenDate { get; set; }
        public string Description { get; set; }
        public string AparatUrl { get; set; }
        public long? Weight { get; set; }
        public int? ProduceDuration { get; set; }
        public bool? UnescoFlag { get; set; }
        public long? UnescoCode { get; set; }
        public bool? MelliFlag { get; set; }
        public long? MelliCode { get; set; }
        public string KeyWords { get; set; }
        public int? Score { get; set; }
        public double? Comission { get; set; }
        public string AuthorName { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string CompareDescription { get; set; }
        public int? OredrDuration { get; set; }
        public string DownloadLink { get; set; }
        public bool? VirtualProduct { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }
        [JsonIgnore]
        public virtual CatProduct CatProduct { get; set; }
        public virtual Status FinalStatus { get; set; }
        public virtual Seller Seller { get; set; }
        public virtual ICollection<CustomerOrderProduct> CustomerOrderProduct { get; set; }
        public virtual ICollection<PackageProduct> PackageProduct { get; set; }
        public virtual ICollection<ProductCatProductParameters> ProductCatProductParameters { get; set; }
        public virtual ICollection<ProductColor> ProductColor { get; set; }
        public virtual ICollection<ProductCustomerRate> ProductCustomerRate { get; set; }
        public virtual ICollection<ProductImage> ProductImage { get; set; }
        public virtual ICollection<ProductLanguage> ProductLanguage { get; set; }
        public virtual ICollection<ProductOffer> ProductOffer { get; set; }
        public virtual ICollection<ProductPackingType> ProductPackingType { get; set; }
        public virtual ICollection<ProductStatusLog> ProductStatusLog { get; set; }
        public virtual ICollection<RelatedProduct> RelatedProductDestinProduct { get; set; }
        public virtual ICollection<RelatedProduct> RelatedProductOriginProduct { get; set; }
    }
}
