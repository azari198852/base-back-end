using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class PostType
    {
        public PostType()
        {
            CustomerOrder = new HashSet<CustomerOrder>();
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
        public long? Price { get; set; }
        public bool? IsFree { get; set; }
        public string ApiUrl { get; set; }
        public string Icon { get; set; }
        public int? Rkey { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual ICollection<CustomerOrder> CustomerOrder { get; set; }
    }
}
