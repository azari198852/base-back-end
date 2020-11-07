using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class ProductDto
    {
        public long Id { get; set; }
        public long? CatProductId { get; set; }
        public string CatProductName { get; set; }
        public long? SellerId { get; set; }
        public string SellerName { get; set; }
        public string Name { get; set; }
        public long? Coding { get; set; }
        public long? FinalStatusId { get; set; }
        public string FinalStatus { get; set; }
        public long? Price { get; set; }
        public long? OfferId { get; set; }
        public long? PriceAftterOffer { get; set; }
        public int? OfferPercent { get; set; }
        public long? OfferAmount { get; set; }
        public long? Count { get; set; }
        public string CoverImageUrl { get; set; }
        public string CoverImageHurl { get; set; }
        public long? SeenCount { get; set; }
        public string LastSeenDate { get; set; }
        public string Description { get; set; }
        public string AparatUrl { get; set; }
        public long? Weight { get; set; }
        public bool? UnescoFlag { get; set; }
        public long? UnescoCode { get; set; }
        public bool? MelliFlag { get; set; }
        public long? MelliCode { get; set; }
        public string KeyWords { get; set; }
        public int? Score { get; set; }
        public float? Rating { get; set; }
    }
}
