using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
   public class OfferRepository : RepositoryBase<Offer>, IOfferRepository
    {
       public OfferRepository(BaseContext repositoryContext)
           : base(repositoryContext)
       {
       }
    }
}
