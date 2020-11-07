using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Contracts
{
   public interface IStatusRepository:IRepositoryBase<Status>
   {
       long GetSatusId(string tableName, long statusTypeRkey);
   }
}
