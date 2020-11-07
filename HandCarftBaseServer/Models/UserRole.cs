using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class UserRole
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? Role { get; set; }
        public long? StartDate { get; set; }
        public long? ExpDate { get; set; }
        public long? FinalStatusId { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual Role RoleNavigation { get; set; }
        public virtual Users User { get; set; }
    }
}
