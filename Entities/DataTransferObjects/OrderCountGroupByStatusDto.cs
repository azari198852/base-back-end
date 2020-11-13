using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
   public class OrderCountGroupByStatusDto
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }
}
