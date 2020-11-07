using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class TablesRepository : RepositoryBase<Tables>, ITablesRepository
    {
        public TablesRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
