using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class CatRole
    {
        public CatRole()
        {
            Role = new HashSet<Role>();
        }

        public long Id { get; set; }
        public long? SystemsId { get; set; }
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

        public virtual Systems Systems { get; set; }
        public virtual ICollection<Role> Role { get; set; }
    }
}
