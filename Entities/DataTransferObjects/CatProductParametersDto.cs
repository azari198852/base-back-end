using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class CatProductParametersDto
    {
        public long Id { get; set; }
        public long? ParametersId { get; set; }
        public long? CatProductId { get; set; }
    }
}
