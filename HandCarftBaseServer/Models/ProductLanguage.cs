using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class ProductLanguage
    {
        public long Id { get; set; }
        public long? LanguageId { get; set; }
        public long? ProductId { get; set; }
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
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual Language Language { get; set; }
        public virtual Product Product { get; set; }
    }
}
