using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ProductCustomerRateRepository : RepositoryBase<ProductCustomerRate>, IProductCustomerRateRepository
    {
        public ProductCustomerRateRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public List<ProductCustomerRate> GetProductCommentFullInfo(long productId)
        {

            var res = FindByCondition(c => c.ProductId == productId && c.Ddate == null && c.DaDate == null)
                .Include(c => c.ProductCustomerRateImage).Include(c => c.Customer).ToList();
            return res;

        }
    }
}
