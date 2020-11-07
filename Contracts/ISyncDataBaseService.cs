using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
  public interface ISyncDataBaseService
  {
      void Sync<T>(string tableName, int serviceType, T entity);
  } 
}
