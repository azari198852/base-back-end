using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class Systems
    {
        public Systems()
        {
            CatApi = new HashSet<CatApi>();
            CatFrom = new HashSet<CatFrom>();
            CatRole = new HashSet<CatRole>();
            Tables = new HashSet<Tables>();
            TablesServiceDiscovery = new HashSet<TablesServiceDiscovery>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Ipaddress { get; set; }
        public int? Port { get; set; }
        public long? Rkey { get; set; }
        public string Url { get; set; }
        public long? FinalStatusId { get; set; }
        public int? Rorder { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual ICollection<CatApi> CatApi { get; set; }
        public virtual ICollection<CatFrom> CatFrom { get; set; }
        public virtual ICollection<CatRole> CatRole { get; set; }
        public virtual ICollection<Tables> Tables { get; set; }
        public virtual ICollection<TablesServiceDiscovery> TablesServiceDiscovery { get; set; }
    }
}
