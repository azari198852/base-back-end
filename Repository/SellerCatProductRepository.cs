using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class SellerCatProductRepository : RepositoryBase<SellerCatProduct>, ISellerCatProductRepository
    {
        public SellerCatProductRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
