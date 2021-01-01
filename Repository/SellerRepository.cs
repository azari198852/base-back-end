using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
   public class SellerRepository : RepositoryBase<Seller>, ISellerRepository
   {
       public SellerRepository(BaseContext repositoryContext)
           : base(repositoryContext)
       {
       }

       public long GetSellerIdByUserId(long userId)
       {

           return RepositoryContext.Seller.Find(userId).Id;
       }
   }
}
