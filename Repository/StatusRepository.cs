using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class StatusRepository : RepositoryBase<Status>, IStatusRepository
    {
        public StatusRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public long GetSatusId(string tableName, long statusTypeRkey)
        {
            return RepositoryContext.Status
                 .Where(c => c.CatStatus.Tables.Any(x => x.Name == tableName) && c.StatusType.Rkey == statusTypeRkey)
                 .Select(c => c.Id).FirstOrDefault();
        }

    }
}
