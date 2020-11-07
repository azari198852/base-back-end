using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
   public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
    {
       public UserRoleRepository(BaseContext repositoryContext)
           : base(repositoryContext)
       {
       }
    }
}
