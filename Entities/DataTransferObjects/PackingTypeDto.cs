using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Entities.DataTransferObjects
{
   public class PackingTypeDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? Price { get; set; }
        public long? Weight { get; set; }
        public List<PackingTypeImageDto> PackingTypeImage { get; set; }
    }
}
