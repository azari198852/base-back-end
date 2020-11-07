using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class ProductColorDto
    {
        public long Id { get; set; }
        public long? ProductId { get; set; }
        public long? ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
    }
}
