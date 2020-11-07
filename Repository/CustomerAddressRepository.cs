using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class CustomerAddressRepository : RepositoryBase<CustomerAddress>, ICustomerAddressRepository
    {
        public CustomerAddressRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
