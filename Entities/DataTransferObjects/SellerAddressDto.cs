using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
   public class SellerAddressDto
    {

        public string Titel { get; set; }
        public string Address { get; set; }
        public string Xgps { get; set; }
        public string Ygps { get; set; }
        public long? ProvinceId { get; set; }
        public long? CityId { get; set; }
        public long? PostalCode { get; set; }
        public long? Tel { get; set; }
        public long? Fax { get; set; }
    }
}
