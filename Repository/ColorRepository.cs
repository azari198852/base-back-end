using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class ColorRepository : RepositoryBase<Color>, IColorRepository
    {
        public ColorRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
