using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class CatDocumentRepositry : RepositoryBase<CatDocument>, ICatDocumentRepositry
    {
        public CatDocumentRepositry(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
