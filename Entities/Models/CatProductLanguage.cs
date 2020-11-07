using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class CatProductLanguage
    {
        public long Id { get; set; }
        public long? LanguageId { get; set; }
        public long? CatProductId { get; set; }
        public long? Pid { get; set; }
        public string Name { get; set; }
        public long? Coding { get; set; }
        public long? Rkey { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual CatProduct CatProduct { get; set; }
        public virtual Language Language { get; set; }
    }
}
