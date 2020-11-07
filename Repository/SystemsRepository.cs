using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
   public class SystemsRepository : RepositoryBase<Systems>, ISystemsRepository
    {
       public SystemsRepository(BaseContext repositoryContext)
           : base(repositoryContext)
       {
       }
    }
}
