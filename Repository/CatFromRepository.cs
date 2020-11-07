using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class CatFromRepository : RepositoryBase<CatFrom>, ICatFromRepository
    {
        public CatFromRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
