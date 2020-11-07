using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class RoleForms
    {
        public RoleForms()
        {
            RoleFormsApi = new HashSet<RoleFormsApi>();
        }

        public long Id { get; set; }
        public long? FormsId { get; set; }
        public long? RoleId { get; set; }
        public long? StartDate { get; set; }
        public long? ExpDate { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual Forms Forms { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<RoleFormsApi> RoleFormsApi { get; set; }
    }
}
