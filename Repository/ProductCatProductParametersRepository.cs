using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
  public class ProductCatProductParametersRepository : RepositoryBase<ProductCatProductParameters>, IProductCatProductParametersRepository
    {
      public ProductCatProductParametersRepository(BaseContext repositoryContext)
          : base(repositoryContext)
      {
      }

      public List<ProductCatProductParameters> GetProductParamList(long productId)
      {
          var result = FindByCondition(c => c.ProductId == productId)
              .Include(c => c.CatProductParameters)
              .ThenInclude(c => c.Parameters).ToList();

          return result;
      }
    }
}
