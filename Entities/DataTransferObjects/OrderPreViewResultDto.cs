using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class OrderPreViewResultDto
    {
        public List<CustomerOrderProduct> ProductsList { get; set; }
        public CustomerOrder Order { get; set; }
    }
}
