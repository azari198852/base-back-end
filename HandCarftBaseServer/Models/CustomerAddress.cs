using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class CustomerAddress
    {
        public CustomerAddress()
        {
            CustomerOrder = new HashSet<CustomerOrder>();
        }

        public long Id { get; set; }
        public long? CustomerId { get; set; }
        public string Titel { get; set; }
        public string Address { get; set; }
        public string Xgps { get; set; }
        public string Ygps { get; set; }
        public long? ProvinceId { get; set; }
        public long? CityId { get; set; }
        public long? PostalCode { get; set; }
        public long? Tel { get; set; }
        public string IssureName { get; set; }
        public string IssureFamily { get; set; }
        public long? IssureMelliCode { get; set; }
        public long? IssureMobile { get; set; }
        public bool? DefualtAddress { get; set; }
        public long? CuserId { get; set; }
        public long? Cdate { get; set; }
        public long? DuserId { get; set; }
        public long? Ddate { get; set; }
        public long? MuserId { get; set; }
        public long? Mdate { get; set; }
        public long? DaUserId { get; set; }
        public long? DaDate { get; set; }

        public virtual Location City { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Location Province { get; set; }
        public virtual ICollection<CustomerOrder> CustomerOrder { get; set; }
    }
}
