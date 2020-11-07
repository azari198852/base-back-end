using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
   public class UsersRepository : RepositoryBase<Users>, IUsersRepository
    {
       public UsersRepository(BaseContext repositoryContext)
           : base(repositoryContext)
       {
       }
    }
}
