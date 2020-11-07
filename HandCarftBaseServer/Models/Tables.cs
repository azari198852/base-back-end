using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class Tables
    {
        public Tables()
        {
            TablesServiceDiscovery = new HashSet<TablesServiceDiscovery>();
        }

        public long Id { get; set; }
        public long? CatStatusId { get; set; }
        public long? SystemsId { get; set; }
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

        public virtual CatStatus CatStatus { get; set; }
        public virtual Systems Systems { get; set; }
        public virtual ICollection<TablesServiceDiscovery> TablesServiceDiscovery { get; set; }
    }
}
