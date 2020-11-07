using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class RoleFormsApi
    {
        public long Id { get; set; }
        public long? RoleFormsId { get; set; }
        public long? FormsApiid { get; set; }
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

        public virtual FormsApi FormsApi { get; set; }
        public virtual RoleForms RoleForms { get; set; }
    }
}
