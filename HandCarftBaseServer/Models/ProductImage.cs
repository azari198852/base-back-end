using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class ProductImage
    {
        public long Id { get; set; }
        public long? ProductId { get; set; }
        public string Title { get; set; }
        public long? FileType { get; set; }
        public string ImageUrl { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual Product Product { get; set; }
    }
}
