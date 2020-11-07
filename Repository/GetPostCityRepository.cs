using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
   public class GetPostCityRepository : RepositoryBase<GetPostCity>, IGetPostCityRepository
    {
       public GetPostCityRepository(BaseContext repositoryContext)
           : base(repositoryContext)
       {
       }
    }
}
