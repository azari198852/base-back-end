using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class ProductParamDto
    {
        public long Id { get; set; }
        public string ParameterName { get; set; }
        public long? CatProductParametersId { get; set; }
        public long? ProductId { get; set; }
        public string Value { get; set; }
    }
}
