using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class RoleFormsRepository : RepositoryBase<RoleForms>, IRoleFormsRepository
    {
        public RoleFormsRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }

    }
}
