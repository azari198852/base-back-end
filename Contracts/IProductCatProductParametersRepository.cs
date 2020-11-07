using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Contracts
{
    public interface IProductCatProductParametersRepository : IRepositoryBase<ProductCatProductParameters>
    {
        List<ProductCatProductParameters> GetProductParamList(long productId);
    }
}
