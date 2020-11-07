using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Entities.BusinessModel
{
    public class OrderProductModel
    {
        public long ProductId { get; set; }
        public int Count { get; set; }
        public long? ColorId { get; set; }
        public long? PackingTypeId { get; set; }
        public long? OfferId { get; set; }

    }
}
