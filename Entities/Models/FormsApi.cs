using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class FormsApi
    {
        public FormsApi()
        {
            RoleFormsApi = new HashSet<RoleFormsApi>();
        }

        public long Id { get; set; }
        public long? Apiid { get; set; }
        public long? FormsId { get; set; }
        public long? FinalStatusId { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual Api Api { get; set; }
        public virtual Forms Forms { get; set; }
        public virtual ICollection<RoleFormsApi> RoleFormsApi { get; set; }
    }
}
