using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class FamousCommentsRepository : RepositoryBase<FamousComments>, IFamousCommentsRepository
    {
        public FamousCommentsRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
