using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class TablesServiceDiscovery
    {
        public long Id { get; set; }
        public long? TablesId { get; set; }
        public long? SystemsId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int? ServiceType { get; set; }
        public int? Port { get; set; }
        public string Description { get; set; }
        public int? Ordering { get; set; }
        public long? FinalStatusId { get; set; }
        public int? Rkey { get; set; }
        public string HashPass { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }

        public virtual Systems Systems { get; set; }
        public virtual Tables Tables { get; set; }
    }
}
