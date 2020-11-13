using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
  public class CustomerOrderProductSampleDto
    {
        public long Id { get; set; }
        public string SellerName { get; set; }
        public string ProductName { get; set; }
        public long? ProductPrice { get; set; }
        public long? ProductOfferPrice { get; set; }
        public int? OrderCount { get; set; }
        public string PackingTypeName { get; set; }
        public long? PackingPrice { get; set; }
        public string CoverImage { get; set; }

    }
}
