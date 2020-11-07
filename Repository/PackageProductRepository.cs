using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
   public class PackageProductRepository : RepositoryBase<PackageProduct>, IPackageProductRepository
    {
       public PackageProductRepository(BaseContext repositoryContext)
           : base(repositoryContext)
       {
       }
    }
}
