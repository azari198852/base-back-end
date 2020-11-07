using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class CustomerOrderProductRepository : RepositoryBase<CustomerOrderProduct>, ICustomerOrderProductRepository
    {
        public CustomerOrderProductRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
