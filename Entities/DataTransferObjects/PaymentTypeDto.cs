using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class PaymentTypeDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long? Rkey { get; set; }
        public string Icon { get; set; }
    }
}
