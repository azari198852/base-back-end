using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models;

namespace Contracts
{
    public interface ICatProductParametersRepository : IRepositoryBase<CatProductParameters>
    {
        void RemoveRange(List<CatProductParameters> list);
    }
}
