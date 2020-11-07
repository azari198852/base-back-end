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
    public class CustomerOrderRepository : RepositoryBase<CustomerOrder>, ICustomerOrderRepository
    {
        public CustomerOrderRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }


        public CustomerOrder GetCustomerOrderFullInfoById(long customerOrderId)
        {

            var res = RepositoryContext.CustomerOrder.Where(c => c.Id == customerOrderId)
                  .Include(c => c.CustomerOrderPayment).ThenInclude(c => c.FinalStatus)
                  .Include(c => c.FinalStatus)
                  .Include(c => c.PaymentType)
                  .Include(c => c.PostType)
                  .Include(c => c.CustomerOrderProduct).ThenInclude(c => c.PackingType)
                  .Include(c => c.CustomerOrderProduct).ThenInclude(c => c.FinalStatus)
                  .Include(c => c.CustomerOrderProduct).ThenInclude(c => c.Seller)
                  .FirstOrDefault();
            return res;

        }

        public List<CustomerOrder> GetCustomerOrderList(long customerId)
        {

            var res = RepositoryContext.CustomerOrder.Where(c => c.CustomerId == customerId && c.Ddate == null && c.DaDate == null)
                .Include(c => c.FinalStatus)
                .Include(c => c.PaymentType)
                .Include(c => c.CustomerOrderPayment).ThenInclude(c => c.FinalStatus)
                .Include(c => c.PostType)
                .OrderByDescending(c => c.Cdate).ToList();
            return res;

        }

    }
}
