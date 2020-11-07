using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class StatusTypeRepository : RepositoryBase<StatusType>, IStatusTypeRepository
    {
        public StatusTypeRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
