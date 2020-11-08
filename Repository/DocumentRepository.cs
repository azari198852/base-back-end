using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
   public class DocumentRepository : RepositoryBase<Document>, IDocumentRepository
    {
        public DocumentRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
