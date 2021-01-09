using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.UIResponse;
using HandCarftBaseServer.Tools;
using Logger;
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
        private readonly ILogHandler _logger;

        public OfferController(IMapper mapper, IRepositoryWrapper repository, ILogHandler logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }


        /// <summary>
        ///لیست تخفیفات 
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("Offer/GetOfferList")]
        public IActionResult GetOfferList()
        {
            try
            {

                var res = _repository.Offer.FindByCondition(c => c.Ddate == null)
                    .Include(c => c.OfferType)
                    .Select(c => new
                    {
                        c.Id,
                        c.Name,
                        FromDate = DateTimeFunc.TimeTickToMiladi(c.FromDate.Value),
                        ToDate = DateTimeFunc.TimeTickToMiladi(c.ToDate.Value),
                        c.MaximumPrice,
                        c.Value,
                        c.Description,
                        Type = c.OfferType.Name,
                        Status = c.DaDate == null ? "فعال" : "غیرفعال"

                    }).OrderByDescending(c => c.Id).ToList();
                _logger.LogData(MethodBase.GetCurrentMethod(), res, null);
                return Ok(res);

            }
            catch (Exception e)
            {
                _logger.LogError(e, MethodBase.GetCurrentMethod());
                return BadRequest(e.Message);

            }
        }

        /// <summary>
        ///لیست محصولات مرتبط به کد تخفیف
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("Offer/GetProductListByOfferId")]
        public IActionResult GetProductListByOfferId(long offerId)
        {
            try
            {

                var prodList = _repository.ProductOffer.FindByCondition(c => c.OfferId == offerId && c.Ddate == null)
                    .Include(c => c.Product)
                    .Select(c => new
                    {
                        ProductId = c.Product.Id,
                        ProductName = c.Product.Name,
                        ProductCoding = c.Product.Coding
                    }).ToList();


                if (prodList.Count == 0)
                    throw new BusinessException(XError.GetDataErrors.NotFound());

                _logger.LogData(MethodBase.GetCurrentMethod(), prodList, null, offerId);
                return Ok(prodList);

            }
            catch (Exception e)
            {
                _logger.LogError(e, MethodBase.GetCurrentMethod(), offerId);
                return BadRequest(e.Message);

            }
        }

        /// <summary>
        ///فعال / غیرفعال کردن تخفیف 
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("Offer/ActiveDeActiveOfferById")]
        public IActionResult ActiveDeActiveOfferById(long offerId)
        {
            try
            {

                var offer = _repository.Offer.FindByCondition(c => c.Id == offerId).FirstOrDefault();
                if (offer == null)
                    throw new BusinessException(XError.GetDataErrors.NotFound());

                offer.DaUserId = ClaimPrincipalFactory.GetUserId(User);
                offer.DaDate ??= DateTime.Now.Ticks;
                if (offer.DaDate != null)
                    offer.DaDate = null;
                _repository.Offer.Update(offer);
                _repository.Save();
                _logger.LogData(MethodBase.GetCurrentMethod(), General.Results_.SuccessMessage(), null, offerId);
                return Ok(General.Results_.SuccessMessage());

            }
            catch (Exception e)
            {
                _logger.LogError(e, MethodBase.GetCurrentMethod(), offerId);
                return BadRequest(e.Message);

            }
        }

        /// <summary>
        ///اطلاعات تخفیف و لیست محصولات براساس آیدی تخفیف
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("Offer/GetOfferFullInfoById")]
        public IActionResult GetOfferFullInfoById(long offerId)
        {
            try
            {

                var offer = _repository.Offer.FindByCondition(c => c.Id == offerId).Include(c => c.OfferType).Select(c => new
                {
                    c.Id,
                    c.Name,
                    FromDate = DateTimeFunc.TimeTickToMiladi(c.FromDate.Value),
                    ToDate = DateTimeFunc.TimeTickToMiladi(c.ToDate.Value),
                    c.MaximumPrice,
                    c.Value,
                    c.Description,
                    c.OfferCode,
                    c.UsedCount,
                    c.UsageCount,
                    c.UsageValue,
                    Type = c.OfferType.Name,
                    Status = c.DaDate == null ? "فعال" : "غیرفعال"

                }).FirstOrDefault();
                if (offer == null)
                    throw new BusinessException(XError.GetDataErrors.NotFound());

                var productIdList = _repository.ProductOffer.FindByCondition(c => c.OfferId == offerId)
                    .Include(c => c.Product).Select(c => c.ProductId);

                var productList = _repository.Product
                    .FindByCondition(c => productIdList.Contains(c.Id) && c.DaDate == null && c.Ddate == null)
                    .Include(c => c.CatProduct)
                    .Include(c => c.Seller)
                    .Select(c => new
                    {
                        c.Id,
                        c.Name,
                        c.Price,
                        c.Coding,
                        CatProduct = c.CatProduct.Name,
                        SellerName = c.Seller.Name + " " + c.Seller.Fname,
                        c.Count
                    }).ToList();

                var res = new { Offer = offer, ProductList = productList };

                _logger.LogData(MethodBase.GetCurrentMethod(), res, null, offerId);
                return Ok(res);

            }
            catch (Exception e)
            {
                _logger.LogError(e, MethodBase.GetCurrentMethod(), offerId);
                return BadRequest(e.Message);

            }
        }

        /// <summary>
        ///ثبت تخفیف جدید
        /// </summary>
        [Authorize]
        [HttpPost]
        [Route("Offer/InsertOffer")]
        public IActionResult InsertOffer(OfferInsertDto offerInsert)
        {
            try
            {
                ParamValidator validator = new ParamValidator();
                validator.ValidateNull(offerInsert.FromDate, General.Messages_.NullInputMessages_.GeneralNullMessage("از تاریخ"))
                .ValidateNull(offerInsert.ToDate, General.Messages_.NullInputMessages_.GeneralNullMessage("تا تاریخ"))
                .ValidateNull(offerInsert.Name, General.Messages_.NullInputMessages_.GeneralNullMessage("عنوان"))
                .ValidateNull(offerInsert.Value, General.Messages_.NullInputMessages_.GeneralNullMessage("مقدار تخفیف"))
                .ValidateNull(offerInsert.MaximumPrice, General.Messages_.NullInputMessages_.GeneralNullMessage("حداکثر قیمت"))
                .ValidateNull(offerInsert.OfferTypeId, General.Messages_.NullInputMessages_.GeneralNullMessage("نوع تخفیف"))
                .ValidateNull(offerInsert.OfferCode, General.Messages_.NullInputMessages_.GeneralNullMessage("کد تخفیف"))
                    .Throw(General.Results_.FieldNullErrorCode());

                var offer = new Offer
                {
                    Cdate = DateTime.Now.Ticks,
                    CuserId = ClaimPrincipalFactory.GetUserId(User),
                    Description = offerInsert.Description,
                    FromDate = offerInsert.FromDate,
                    HaveTimer = offerInsert.HaveTimer,
                    MaximumPrice = offerInsert.MaximumPrice,
                    Name = offerInsert.Name,
                    OfferCode = offerInsert.OfferCode,
                    OfferTypeId = offerInsert.OfferTypeId,
                    ToDate = offerInsert.ToDate,
                    Value = offerInsert.Value
                };
                offerInsert.ProductIdList.ForEach(c =>
                {
                    var productOffer = new ProductOffer
                    {
                        Cdate = DateTime.Now.Ticks,
                        CuserId = ClaimPrincipalFactory.GetUserId(User),
                        ProductId = c,
                        Value = offerInsert.Value,
                        FromDate = offerInsert.FromDate,
                        ToDate = offerInsert.ToDate
                    };
                    offer.ProductOffer.Add(productOffer);
                });
                _repository.Offer.Create(offer);
                _repository.Save();
                _logger.LogData(MethodBase.GetCurrentMethod(), offer.Id, null, offerInsert);
                return Ok(offer.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, MethodBase.GetCurrentMethod(), offerInsert);
                return BadRequest(e.Message);
            }
        }


        /// <summary>
        ///ویرایش تخفیف
        /// </summary>
        [Authorize]
        [HttpPut]
        [Route("Offer/UpdateOffer")]
        public IActionResult UpdateOffer(OfferInsertDto offerInsert)
        {
            try
            {
                var validator = new ParamValidator();
                validator.ValidateNull(offerInsert.FromDate, General.Messages_.NullInputMessages_.GeneralNullMessage("از تاریخ"))
                .ValidateNull(offerInsert.Id, General.Messages_.NullInputMessages_.GeneralNullMessage("آیدی"))
                .ValidateNull(offerInsert.ToDate, General.Messages_.NullInputMessages_.GeneralNullMessage("تا تاریخ"))
                .ValidateNull(offerInsert.Name, General.Messages_.NullInputMessages_.GeneralNullMessage("عنوان"))
                .ValidateNull(offerInsert.Value, General.Messages_.NullInputMessages_.GeneralNullMessage("مقدار تخفیف"))
                .ValidateNull(offerInsert.MaximumPrice, General.Messages_.NullInputMessages_.GeneralNullMessage("حداکثر قیمت"))
                .ValidateNull(offerInsert.OfferTypeId, General.Messages_.NullInputMessages_.GeneralNullMessage("نوع تخفیف"))
                .ValidateNull(offerInsert.OfferCode, General.Messages_.NullInputMessages_.GeneralNullMessage("کد تخفیف"))
                    .Throw(General.Results_.FieldNullErrorCode());

                var offer = _repository.Offer.FindByCondition(c => c.Id == offerInsert.Id).FirstOrDefault();
                if (offer == null)
                    throw new BusinessException(XError.GetDataErrors.NotFound());

                offer.Mdate = DateTime.Now.Ticks;
                offer.MuserId = ClaimPrincipalFactory.GetUserId(User);
                offer.Description = offerInsert.Description;
                offer.FromDate = offerInsert.FromDate;
                offer.HaveTimer = offerInsert.HaveTimer;
                offer.MaximumPrice = offerInsert.MaximumPrice;
                offer.Name = offerInsert.Name;
                offer.OfferCode = offerInsert.OfferCode;
                offer.OfferTypeId = offerInsert.OfferTypeId;
                offer.ToDate = offerInsert.ToDate;
                offer.Value = offerInsert.Value;

                var deletedPRoductOffer = _repository.ProductOffer.FindByCondition(c => c.OfferId == offerInsert.Id)
                    .ToList();
                deletedPRoductOffer.ForEach(c =>
                {
                    _repository.ProductOffer.Delete(c);
                });



                offerInsert.ProductIdList.ForEach(c =>
                {
                    var productOffer = new ProductOffer
                    {
                        Cdate = DateTime.Now.Ticks,
                        CuserId = ClaimPrincipalFactory.GetUserId(User),
                        ProductId = c,
                        Value = offerInsert.Value,
                        FromDate = offerInsert.FromDate,
                        ToDate = offerInsert.ToDate
                    };
                    offer.ProductOffer.Add(productOffer);
                });
                _repository.Offer.Update(offer);
                _repository.Save();
                _logger.LogData(MethodBase.GetCurrentMethod(), General.Results_.SuccessMessage(), null, offerInsert);
                return Ok(General.Results_.SuccessMessage());
            }
            catch (Exception e)
            {
                _logger.LogError(e, MethodBase.GetCurrentMethod(), offerInsert);
                return BadRequest(e.Message);
            }
        }


        /// <summary>
        ///حذف تخفیف
        /// </summary>
        [Authorize]
        [HttpDelete]
        [Route("Offer/DeleteOffer")]
        public IActionResult DeleteOffer(long offerId)
        {
            try
            {


                var offer = _repository.Offer.FindByCondition(c => c.Id == offerId).FirstOrDefault();
                if (offer == null)
                    throw new BusinessException(XError.GetDataErrors.NotFound());

                offer.Ddate = DateTime.Now.Ticks;
                offer.DaUserId = ClaimPrincipalFactory.GetUserId(User);

                var deletedPRoductOffer = _repository.ProductOffer.FindByCondition(c => c.OfferId == offerId)
                    .ToList();
                deletedPRoductOffer.ForEach(c =>
                {
                    c.Ddate = DateTime.Now.Ticks;
                    c.DaUserId = ClaimPrincipalFactory.GetUserId(User);
                    _repository.ProductOffer.Update(c);
                });

                _repository.Offer.Update(offer);
                _repository.Save();
                _logger.LogData(MethodBase.GetCurrentMethod(), General.Results_.SuccessMessage(), null, offerId);
                return Ok(General.Results_.SuccessMessage());
            }
            catch (Exception e)
            {
                _logger.LogError(e, MethodBase.GetCurrentMethod(), offerId);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///لیست نوع تخفیف
        /// </summary>
        [Authorize]
        [HttpDelete]
        [Route("Offer/GetOfferTypeList")]
        public IActionResult GetOfferTypeList()
        {
            try
            {


                var res = _repository.OfferType.FindByCondition(c => c.DaDate == null && c.Ddate == null)
                    .Select(c => new { c.Id, c.Name, c.CustomerOffer, c.ProductOffer, c.PublicOffer }).ToList();

                _logger.LogData(MethodBase.GetCurrentMethod(), res, null);
                return Ok(res);
            }
            catch (Exception e)
            {
                _logger.LogError(e, MethodBase.GetCurrentMethod());
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///دریافت سابقه تخفیفات محصول
        /// </summary>
        [Authorize]
        [HttpDelete]
        [Route("Offer/GetProductOfferHistoryList")]
        public IActionResult GetProductOfferHistoryList(long productId)
        {
            try
            {


                var offerlist = _repository.ProductOffer
                    .FindByCondition(c => c.Ddate == null && c.ProductId == productId)
                    .Select(c => c.OfferId);

                var res = _repository.Offer
                    .FindByCondition(c =>  c.Ddate == null && offerlist.Contains(c.Id)).Include(c=>c.OfferType)
                    .Select(c => new
                    {
                        c.Id,
                        c.Name,
                        FromDate = DateTimeFunc.TimeTickToMiladi(c.FromDate.Value),
                        ToDate = DateTimeFunc.TimeTickToMiladi(c.ToDate.Value),
                        c.MaximumPrice,
                        c.Value,
                        c.Description,
                        Type = c.OfferType.Name,
                        Status = c.DaDate == null ? "فعال" : "غیرفعال"

                    }).OrderByDescending(c => c.Id).ToList();


                _logger.LogData(MethodBase.GetCurrentMethod(), res, null);
                return Ok(res);
            }
            catch (Exception e)
            {
                _logger.LogError(e, MethodBase.GetCurrentMethod());
                return BadRequest(e.Message);
            }
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
                _logger.LogError(e, MethodBase.GetCurrentMethod(), offerCode);
                return ListResult<OfferDto>.GetFailResult(null);

            }
        }

        #endregion

    }
}
