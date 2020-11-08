using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class CatDocument
    {
        public CatDocument()
        {
            Document = new HashSet<Document>();
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public int? Rkey { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual ICollection<Document> Document { get; set; }
    }
}
