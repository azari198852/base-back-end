using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Entities.DataTransferObjects
{
    public class ProductPackingTypeDto
    {
        public long Id { get; set; }
        public long? ProductId { get; set; }
        public long? PackinggTypeId { get; set; }
        public string PackingTypeName { get; set; }
        public long? Price { get; set; }
        public long? Weight { get; set; }
        List<ProductPackingTypeImage> ProductPackingTypeImage { get; set; }
    }
}
