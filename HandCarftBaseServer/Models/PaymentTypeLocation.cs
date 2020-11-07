using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class PaymentTypeLocation
    {
        public long Id { get; set; }
        public long? PaymentTypeId { get; set; }
        public long? LocationId { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual Location Location { get; set; }
        public virtual PaymentType PaymentType { get; set; }
    }
}
