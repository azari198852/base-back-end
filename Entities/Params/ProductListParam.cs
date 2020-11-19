using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Params
{
    public class ProductListParam
    {

        public long? CatProductId { get; set; }
        public string ProductName { get; set; }
        public long? MinPrice { get; set; }
        public long? MaxPrice { get; set; }
        public short SortMethod { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public List<long> SellerIdList { get; set; }
    }
}
