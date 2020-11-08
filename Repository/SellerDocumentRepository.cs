using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
   public class SellerDocumentRepository : RepositoryBase<SellerDocument>, ISellerDocumentRepository
    {
        public SellerDocumentRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
