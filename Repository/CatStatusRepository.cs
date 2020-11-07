using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class CatStatusRepository : RepositoryBase<CatStatus>, ICatStatusRepository
    {
        public CatStatusRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
