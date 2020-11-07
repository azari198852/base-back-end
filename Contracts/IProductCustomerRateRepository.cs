using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Contracts
{
   public interface IProductCustomerRateRepository:IRepositoryBase<ProductCustomerRate>
   {
       List<ProductCustomerRate> GetProductCommentFullInfo(long productId);
   }
}
