using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
  public  class OfferTypeRepository : RepositoryBase<OfferType>, IOfferTypeRepository
    {
        public OfferTypeRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
