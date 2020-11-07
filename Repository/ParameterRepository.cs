using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
   public class ParameterRepository : RepositoryBase<Parameters>, IParameterRepository
   {
       public ParameterRepository(BaseContext repositoryContext)
           : base(repositoryContext)
       {
       }
    }
}
