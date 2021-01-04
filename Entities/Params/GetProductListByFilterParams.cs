using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Params
{
    public class GetProductListByFilterParams
    {
        public List<long> CatProductIds { get; set; }
        public List<long> SellerIds { get; set; }
        public List<long> ProductIds { get; set; }
        public long? FromPrice { get; set; }
        public long? ToPrice { get; set; }
    }
}
