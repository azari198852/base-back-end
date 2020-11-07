using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.UIResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HandCarftBaseServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class PaymentTypeController : ControllerBase
    {
        public IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public PaymentTypeController(IMapper mapper, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        /// <summary>
        ///لیست شیوه های پرداخت 
        /// </summary>
        [HttpGet]
        [Route("PaymentType/GetPaymentTypeList_UI")]
        public ListResult<PaymentTypeDto> GetPaymentTypeList_UI()
        {

            try
            {
                var res = _repository.PaymentType.FindByCondition(c => c.Ddate == null && c.DaDate == null).ToList();
                var result = _mapper.Map<List<PaymentTypeDto>>(res);
                var finalresult = ListResult<PaymentTypeDto>.GetSuccessfulResult(result);
                return finalresult;
            }
            catch (Exception e)
            {
                return ListResult<PaymentTypeDto>.GetFailResult(e.Message);
            }
        }
    }
}
