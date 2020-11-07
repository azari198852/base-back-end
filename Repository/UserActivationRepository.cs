using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class UserActivationRepository : RepositoryBase<UserActivation>, IUserActivationRepository
    {
        public UserActivationRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
