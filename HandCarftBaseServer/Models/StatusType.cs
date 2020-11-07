using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class StatusType
    {
        public StatusType()
        {
            Status = new HashSet<Status>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public int? Rkey { get; set; }
        public string Description { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }

        public virtual ICollection<Status> Status { get; set; }
    }
}
