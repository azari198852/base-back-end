using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class Api
    {
        public Api()
        {
            FormsApi = new HashSet<FormsApi>();
        }

        public long Id { get; set; }
        public long? CatApiid { get; set; }
        public string Name { get; set; }
        public long? Rkey { get; set; }
        public string Url { get; set; }
        public int? Type { get; set; }
        public string Description { get; set; }
        public long? FinalStatusId { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual CatApi CatApi { get; set; }
        public virtual ICollection<FormsApi> FormsApi { get; set; }
    }
}
