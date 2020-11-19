using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
   public class WorkRepository : RepositoryBase<Work>, IWorkRepository
    {
       public WorkRepository(BaseContext repositoryContext)
           : base(repositoryContext)
       {
       }
    }
}
