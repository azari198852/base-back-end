using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
   public class SellerAddressRepository : RepositoryBase<SellerAddress>, ISellerAddressRepository
    {
        public SellerAddressRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
