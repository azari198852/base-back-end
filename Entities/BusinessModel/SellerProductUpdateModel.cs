using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.BusinessModel
{
    public class SellerProductUpdateModel
    {
        public long ProductId { get; set; }
        public long? Price { get; set; }
        public long? ProducePrice { get; set; }
        public int? ProduceDuration { get; set; }
    }
}
