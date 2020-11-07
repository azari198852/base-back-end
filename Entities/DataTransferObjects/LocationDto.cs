using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
  public class LocationDto
    {
        public long Id { get; set; }
        public long? Pid { get; set; }
        public string Name { get; set; }
        public string EnName { get; set; }
        public long? LocationCode { get; set; }

    }
}
