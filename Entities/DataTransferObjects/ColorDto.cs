using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class ColorDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ColorCode { get; set; }
        public long? Rkey { get; set; }
    }
}
