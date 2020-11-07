using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class Work
    {
        public Work()
        {
            Customer = new HashSet<Customer>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Rkey { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual ICollection<Customer> Customer { get; set; }
    }
}
