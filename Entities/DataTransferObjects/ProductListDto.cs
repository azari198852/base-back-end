using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
   public class ProductListDto
    {
        public List<ProductDto> ProductList { set; get; }
        public int TotalCount { set; get; }
        public long? MinPrice { get; set; }
        public long? MaxPrice { get; set; }

    }
}
