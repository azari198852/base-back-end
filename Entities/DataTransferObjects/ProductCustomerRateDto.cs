using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class ProductCustomerRateDto
    {
        public long Id { get; set; }
        public long? Pid { get; set; }
        public long? ProductId { get; set; }
        public long? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int? Rate { get; set; }
        public long? CommentDate { get; set; }
        public string CommentDesc { get; set; }
        public int? LikeCount { get; set; }
        public int? DisLikeCount { get; set; }
        public List<ProductCustomerRateImageDto> ProductCustomerRateImages { get; set; }
    }
}
