using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
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
    public class OfferController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILogger<OfferController> _logger;

        public OfferController(IMapper mapper, IRepositoryWrapper repository, ILogger<OfferController> logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }

     
        #region UI_Methods

        /// <summary>
        ///بررسی کد تخفیف 
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("Offer/GetOfferValueByCode_UI")]
        public ListResult<OfferDto> GetOfferValueByCode_UI(string offerCode)
        {
            try
            {

                var time = DateTime.Now.Ticks;
                var offer = _repository.Offer.FindByCondition(c => c.OfferCode == offerCode && c.Ddate == null && c.DaDate == null).Include(c => c.OfferType).FirstOrDefault();
                if (offer == null) return ListResult<OfferDto>.GetFailResult("کد تخفیف وارد شده صحیح نمی باشد!");
                if (offer.OfferType.PublicOffer == true)
                {
                    if (offer.FromDate > time || time > offer.ToDate) return ListResult<OfferDto>.GetFailResult("کد تخفیف وارد شده در تاریخ جاری معتبر نمی باشد!");
                    var result = _mapper.Map<List<OfferDto>>(offer);
                    var finalresult = ListResult<OfferDto>.GetSuccessfulResult(result);
                    return finalresult;
                }
                else
                {
                    var userId = ClaimPrincipalFactory.GetUserId(User);
                    var customerId = _repository.Customer.FindByCondition(c => c.UserId == userId).Select(c => c.Id)
                        .FirstOrDefault();
                    var customerOffer = _repository.CustomerOffer.FindByCondition(c =>
                              c.DaDate == null && c.Ddate == null && c.CustomerId == customerId && c.OfferId == offer.Id)
                         .FirstOrDefault();
                    if (customerOffer == null) return ListResult<OfferDto>.GetFailResult("کد تخفیف وارد شده صحیح نمی باشد!");
                    if (customerOffer.FromDate > time || customerOffer.ToDate < time) return ListResult<OfferDto>.GetFailResult("کد تخفیف وارد شده در تاریخ جاری معتبر نمی باشد!");
                    if (offer.UsageCount != null && !(offer.UsageCount > customerOffer.UsageCount))
                        return ListResult<OfferDto>.GetFailResult(
                            "تعداد دفعات استفاده از کد تخفیف به پایان رسیده است!");
                    var result = _mapper.Map<List<OfferDto>>(customerOffer);
                    var finalresult = ListResult<OfferDto>.GetSuccessfulResult(result);
                    return finalresult;


                }


            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return ListResult<OfferDto>.GetFailResult(null);

            }
        }

        #endregion

    }
}
