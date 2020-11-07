using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
   public class ProductColorRepository : RepositoryBase<ProductColor>, IProductColorRepository
    {
       public ProductColorRepository(BaseContext repositoryContext)
           : base(repositoryContext)
       {
       }
    }
}
