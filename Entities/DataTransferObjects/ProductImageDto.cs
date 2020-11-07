using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
   public class ProductImageDto
    {
        public long Id { get; set; }
        public long? ProductId { get; set; }
        public string Title { get; set; }
        public long? FileType { get; set; }
        public string ImageUrl { get; set; }
    }
}
