using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class Role
    {
        public Role()
        {
            InverseP = new HashSet<Role>();
            RoleForms = new HashSet<RoleForms>();
            UserRole = new HashSet<UserRole>();
        }

        public long Id { get; set; }
        public long? Pid { get; set; }
        public long? CatRoleId { get; set; }
        public string Name { get; set; }
        public int? Rkey { get; set; }
        public int? MaxUser { get; set; }
        public long? ExpDate { get; set; }
        public long? StDate { get; set; }
        public long? FinalStatusId { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual CatRole CatRole { get; set; }
        public virtual Role P { get; set; }
        public virtual ICollection<Role> InverseP { get; set; }
        public virtual ICollection<RoleForms> RoleForms { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
