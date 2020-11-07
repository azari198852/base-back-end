using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.BusinessModel
{
    public class OrderModel
    {
        public long PaymentTypeId { get; set; }
        public long? PostTypeId { get; set; }
        public long? OfferId { get; set; }
        public long CustomerAddressId { get; set; }
        public string CustomerDescription { get; set; }
        public List<OrderProductModel> ProductList { get; set; }

    }
}
