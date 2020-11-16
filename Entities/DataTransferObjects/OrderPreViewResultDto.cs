using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class OrderPreViewResultDto
    {
        public List<CustomerOrderProduct> ProductsList { get; set; }

        public long? PostServicePrice { get; set; }
        public int? OfferValue { get; set; }
        public long? OfferPrice { get; set; }
        public long? OrderPrice { get; set; }
        public long? FinalPrice { get; set; }
        public long? PaymentTypeId { get; set; }
        public long? PackingPrice { get; set; }
        public long? PackingWeight { get; set; }
        public long? OrderWeight { get; set; }
        public long? FinalWeight { get; set; }

        public List<CustomerOrderProduct> ProductList { get; set; }
    }
}
