using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
   public class CustomerAddressDto
    {
        public long Id { get; set; }
        public long? CustomerId { get; set; }
        public string Titel { get; set; }
        public string Address { get; set; }
        public string Xgps { get; set; }
        public string Ygps { get; set; }
        public long? ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public long? CityId { get; set; }
        public string CityName { get; set; }
        public long? PostalCode { get; set; }
        public long? Tel { get; set; }
        public string IssureName { get; set; }
        public string IssureFamily { get; set; }
        public long? IssureMelliCode { get; set; }
        public long? IssureMobile { get; set; }
        public bool? DefualtAddress { get; set; }
    }
}
