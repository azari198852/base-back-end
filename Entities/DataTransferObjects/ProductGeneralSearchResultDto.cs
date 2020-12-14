using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
   public class ProductGeneralSearchResultDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public long CatProductId { get; set; }
        public long CatProductCode { get; set; }
        public string CatProductName { get; set; }
      
    }
}
