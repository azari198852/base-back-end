using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities.Models;

namespace Contracts
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        IQueryable<Product> GetProductListFullInfo();
    }
}
