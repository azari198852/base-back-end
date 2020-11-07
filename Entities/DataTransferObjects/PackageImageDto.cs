using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
   public class PackageImageDto
    {
        public long Id { get; set; }
        public long? PackageId { get; set; }
        public long? FileType { get; set; }
        public string ImageUrl { get; set; }
    }
}
