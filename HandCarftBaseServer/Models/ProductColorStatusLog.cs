using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class ProductColorStatusLog
    {
        public long Id { get; set; }
        public long? ProductColorId { get; set; }
        public long? StatusId { get; set; }
        public long? Price { get; set; }
        public long? ProducePrice { get; set; }
        public int? ProduceDuration { get; set; }
        public long? Count { get; set; }
        public long? ChangeDate { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual ProductColor ProductColor { get; set; }
        public virtual Status Status { get; set; }
    }
}
