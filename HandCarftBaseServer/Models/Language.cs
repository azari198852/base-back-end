using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class Language
    {
        public Language()
        {
            CatProductLanguage = new HashSet<CatProductLanguage>();
            ProductLanguage = new HashSet<ProductLanguage>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string FlagIcon { get; set; }
        public string ShortName { get; set; }
        public long? Rkey { get; set; }
        public bool? IsRtl { get; set; }
        public long? FinalStatusId { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual ICollection<CatProductLanguage> CatProductLanguage { get; set; }
        public virtual ICollection<ProductLanguage> ProductLanguage { get; set; }
    }
}
