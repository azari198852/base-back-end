using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Contracts
{
   public interface ICustomerOrderRepository:IRepositoryBase<CustomerOrder>
   {
       CustomerOrder GetCustomerOrderFullInfoById(long customerOrderId);
       List<CustomerOrder> GetCustomerOrderList(long customerId);
   }
}
