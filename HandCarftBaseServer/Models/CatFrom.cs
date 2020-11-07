using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class CatFrom
    {
        public CatFrom()
        {
            Forms = new HashSet<Forms>();
        }

        public long Id { get; set; }
        public long? SystemsId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public long? Rkey { get; set; }
        public string Url { get; set; }
        public int? Rorder { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual Systems Systems { get; set; }
        public virtual ICollection<Forms> Forms { get; set; }
    }
}
