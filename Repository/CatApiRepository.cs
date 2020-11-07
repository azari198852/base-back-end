using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class CatApiRepository : RepositoryBase<CatApi>, ICatApiRepository
    {
        public CatApiRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
