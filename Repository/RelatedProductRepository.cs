using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
   public class RelatedProductRepository : RepositoryBase<RelatedProduct>, IRelatedProductRepository
    {
       public RelatedProductRepository(BaseContext repositoryContext)
           : base(repositoryContext)
       {
       }
    }
}
