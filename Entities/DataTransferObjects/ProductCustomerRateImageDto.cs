using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class ProductCustomerRateImageDto
    {
        public long Id { get; set; }
        public long? ProductCustomerRateId { get; set; }
        public long? FileType { get; set; }
        public string FileUrl { get; set; }
    }
}
