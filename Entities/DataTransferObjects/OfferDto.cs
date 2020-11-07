using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
   public class OfferDto
    {
        public long OfferId { get; set; }
        public long? CustomerOfferId { get; set; }
        public string Name { get; set; }
        public double? Value { get; set; }
        public string OfferCode { get; set; }
        public long? MaximumPrice { get; set; }
        public long? MaximumUsagePrice { get; set; }
        public double? UsageValue { get; set; }

    }
}
