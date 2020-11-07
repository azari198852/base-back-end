using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class Parameters
    {
        public Parameters()
        {
            CatProductParameters = new HashSet<CatProductParameters>();
            InverseP = new HashSet<Parameters>();
        }

        public long Id { get; set; }
        public long? Pid { get; set; }
        public string Name { get; set; }
        public long? Rkey { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual Parameters P { get; set; }
        public virtual ICollection<CatProductParameters> CatProductParameters { get; set; }
        public virtual ICollection<Parameters> InverseP { get; set; }
    }
}
