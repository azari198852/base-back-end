using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class CustomerStatusLog
    {
        public long Id { get; set; }
        public long? CustomerId { get; set; }
        public long? StatusId { get; set; }
        public long? ChangeDate { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Status Status { get; set; }
    }
}
