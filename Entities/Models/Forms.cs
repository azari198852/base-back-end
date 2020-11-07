using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class Forms
    {
        public Forms()
        {
            FormsApi = new HashSet<FormsApi>();
            RoleForms = new HashSet<RoleForms>();
        }

        public long Id { get; set; }
        public long? CatFromsId { get; set; }
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

        public virtual CatFrom CatFroms { get; set; }
        public virtual ICollection<FormsApi> FormsApi { get; set; }
        public virtual ICollection<RoleForms> RoleForms { get; set; }
    }
}
