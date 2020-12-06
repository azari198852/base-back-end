using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class CatProductParametersRepository : RepositoryBase<CatProductParameters>, ICatProductParametersRepository
    {
        public CatProductParametersRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public void RemoveRange(List<CatProductParameters> list)
        {
            RepositoryContext.CatProductParameters.RemoveRange(list);
            
        }
    }
}
