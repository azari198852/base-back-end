using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Entities.Models
{
    public partial class ProductCustomerRate
    {
        public ProductCustomerRate()
        {
            InverseP = new HashSet<ProductCustomerRate>();
            ProductCustomerRateImage = new HashSet<ProductCustomerRateImage>();
        }

        public long Id { get; set; }
        public long? Pid { get; set; }
        public long? ProductId { get; set; }
        public long? CustomerId { get; set; }
        public int? Rate { get; set; }
        public long? CommentDate { get; set; }
        public string CommentDesc { get; set; }
        public int? LikeCount { get; set; }
        public int? DisLikeCount { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual Customer Customer { get; set; }
        [JsonIgnore]
        public virtual ProductCustomerRate P { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<ProductCustomerRate> InverseP { get; set; }
        public virtual ICollection<ProductCustomerRateImage> ProductCustomerRateImage { get; set; }
    }
}
