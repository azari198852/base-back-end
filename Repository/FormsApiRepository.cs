using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
   public class FormsApiRepository : RepositoryBase<FormsApi>, IFormsApiRepository
    {
       public FormsApiRepository(BaseContext repositoryContext)
           : base(repositoryContext)
       {
       }
    }
}
