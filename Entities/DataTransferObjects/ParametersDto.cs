using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
   public class ParametersDto
    {
        public long Id { get; set; }
        public long? Pid { get; set; }
        public string Name { get; set; }
        public long? Rkey { get; set; }
    }
}
