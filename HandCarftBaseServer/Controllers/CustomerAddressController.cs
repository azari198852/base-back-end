using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.UIResponse;
using HandCarftBaseServer.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HandCarftBaseServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CustomerAddressController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILogger<CustomerAddressController> _logger;

        public CustomerAddressController(IMapper mapper, IRepositoryWrapper repository, ILogger<CustomerAddressController> logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        ///آدرس پیشفرض مشتری با استفاده از توکن
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("CustomerAddress/GetCustomerDefultAddress_UI")]
        public SingleResult<CustomerAddressDto> GetCustomerDefultAddress_UI()
        {
            try
            {
                var userId = ClaimPrincipalFactory.GetUserId(User);
                var address = _repository.CustomerAddress
                    .FindByCondition(c => c.Customer.UserId == userId && c.DefualtAddress == true && c.Ddate == null && c.DaDate == null).Include(c => c.City).Include(c => c.Province).FirstOrDefault();
                if (address == null) return SingleResult<CustomerAddressDto>.GetFailResult("آدرس پیشفرضی یافت نشد!");

                var result = _mapper.Map<CustomerAddressDto>(address);
                var finalresult = SingleResult<CustomerAddressDto>.GetSuccessfulResult(result);
                return finalresult;

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);


                return SingleResult<CustomerAddressDto>.GetFailResult(e.Message);
            }

        }

        /// <summary>
        ///لیست آدرس های مشتری با استفاده از توکن
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("CustomerAddress/GetCustomerAddressList_UI")]
        public ListResult<CustomerAddressDto> GetCustomerAddressList_UI()
        {
            try
            {
                var userId = ClaimPrincipalFactory.GetUserId(User);
                var address = _repository.CustomerAddress
                    .FindByCondition(c => c.Ddate == null && c.DaDate == null && c.Customer.UserId == userId && c.DefualtAddress == true).Include(c => c.City).Include(c => c.Province).ToList();


                var result = _mapper.Map<List<CustomerAddressDto>>(address);
                var finalresult = ListResult<CustomerAddressDto>.GetSuccessfulResult(result, result.Count);
                return finalresult;

            }
            catch (Exception e)
            {


                return ListResult<CustomerAddressDto>.GetFailResult(e.Message);
            }

        }

        /// <summary>
        ///افزودن آدرس جدید برای مشتری
        /// </summary>
        [Authorize]
        [HttpPost]
        [Route("CustomerAddress/InsertCustomerAddress_UI")]
        public SingleResult<long> InsertCustomerAddress_UI(CustomerAddressDto addressdto)
        {
            try
            {
                var userId = ClaimPrincipalFactory.GetUserId(User);
                var customerId = _repository.Customer.FindByCondition(c => c.UserId == userId).Select(c => c.Id)
                    .FirstOrDefault();
                var address = _mapper.Map<CustomerAddress>(addressdto);

                address.CustomerId = customerId;
                address.CuserId = userId;
                address.Cdate = DateTime.Now.Ticks;
                _repository.CustomerAddress.Create(address);
                if (address.DefualtAddress.HasValue && address.DefualtAddress.Value)
                {
                    var addresslist = _repository.CustomerAddress.FindByCondition(c => c.CustomerId == customerId && c.Ddate == null && c.DaDate == null && c.DefualtAddress == true)
                        .FirstOrDefault();
                    if (addresslist != null)
                    {
                        addresslist.DefualtAddress = false;
                        addresslist.Mdate = DateTime.Now.Ticks;
                        addresslist.MuserId = userId;
                        _repository.CustomerAddress.Update(addresslist);
                    }

                }

                _repository.Save();
                return SingleResult<long>.GetSuccessfulResult(address.Id);

            }
            catch (Exception e)
            {


                return SingleResult<long>.GetFailResult(e.Message);
            }

        }
    }
}
