using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public partial class PaymentType
    {
        public PaymentType()
        {
            CustomerOrder = new HashSet<CustomerOrder>();
            PaymentTypeLocation = new HashSet<PaymentTypeLocation>();
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long? Rkey { get; set; }
        public string Icon { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual ICollection<CustomerOrder> CustomerOrder { get; set; }
        public virtual ICollection<PaymentTypeLocation> PaymentTypeLocation { get; set; }
    }
}
