using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.BusinessModel;
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
using Newtonsoft.Json;

namespace HandCarftBaseServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILogHandler _logger;

        public SellerController(IMapper mapper, IRepositoryWrapper repository, ILogHandler logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }


        [HttpGet]
        [Route("Seller/GetSellerList")]
        public IActionResult GetFamousCommentsList()
        {
            try
            {
                var res = _repository.Seller.FindByCondition(c => c.DaDate == null && c.Ddate == null)
                    .ToList();
                _logger.LogData(MethodBase.GetCurrentMethod(), res, null);
                return Ok(res);
            }
            catch (Exception e)
            {
                _logger.LogError(e, MethodBase.GetCurrentMethod());
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        [Route("Seller/GetSellerListForGrid")]
        public IActionResult GetSellerListForGrid()
        {
            try
            {
                var res = _repository.Seller.FindByCondition(c => c.DaDate == null && c.Ddate == null).Include(c => c.FinalStatus).Select(c => new
                {
                    c.Id,
                    c.SellerCode,
                    Type = c.RealOrLegal == 1 ? "حقیقی" : "حقوقی",
                    Fullname = c.Name + " " + c.Fname,
                    c.MelliCode,
                    c.Mobile,
                    c.Tel,
                    Status = c.FinalStatus.Name

                })
                    .ToList();
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
        /// لیست سفارشات فروشنده  
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("Seller/GetSellerOrderList")]
        public IActionResult GetSellerOrderList()
        {
            try
            {
                var userId = ClaimPrincipalFactory.GetUserId(User);
                var sellerId = _repository.Seller.GetSellerIdByUserId(userId);
                var productList = _repository.CustomerOrderProduct.FindByCondition(c => c.Product.SellerId == sellerId)
                    .Include(c => c.FinalStatus)
                    .Include(c => c.CustomerOrder)
                    .Select(c => new { c.Id, c.ProductCode, c.ProductName, c.CustomerOrder.OrderNo, c.OrderCount, c.ProductPrice, c.FinalStatus.Name })
                    .OrderByDescending(c => c.Id).ToList();

                _logger.LogData(MethodBase.GetCurrentMethod(), productList, null);
                return Ok(productList);
            }
            catch (Exception e)
            {
                _logger.LogError(e, MethodBase.GetCurrentMethod());
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///  تغییر وضعیت کالای سفارش به آماده ارسال به مرکز پردازش   
        /// </summary>
        [Authorize]
        [HttpPut]
        [Route("Seller/UpdateStatusToReadyToSend")]
        public IActionResult UpdateStatusToReadyToSend(long customerOrderProductId)
        {
            try
            {
                var userId = ClaimPrincipalFactory.GetUserId(User);
                var sellerId = _repository.Seller.GetSellerIdByUserId(userId);

                var product = _repository.CustomerOrderProduct.FindByCondition(c => c.Id == customerOrderProductId)
                    .FirstOrDefault();
                if (product == null)
                {
                    throw new BusinessException(XError.GetDataErrors.NotFound());
                }

                if (product.FinalStatusId != 22)
                {
                    throw new BusinessException(XError.BusinessErrors.InvalidOrderProductStatus());
                }

                product.FinalStatusId = 23;
                product.Mdate = DateTime.Now.Ticks;
                product.MuserId = userId;
                _repository.CustomerOrderProduct.Update(product);
                _repository.Save();

                _logger.LogData(MethodBase.GetCurrentMethod(), General.Results_.SuccessMessage(), null, customerOrderProductId);
                return Ok(General.Results_.SuccessMessage());
            }
            catch (Exception e)
            {
                _logger.LogError(e, MethodBase.GetCurrentMethod(), customerOrderProductId);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        ///  تغییر وضعیت کالای سفارش به ارسال به مرکز پردازش   
        /// </summary>
        [Authorize]
        [HttpPut]
        [Route("Seller/UpdateStatusToSent")]
        public IActionResult UpdateStatusToSent(long customerOrderProductId)
        {
            try
            {
                var userId = ClaimPrincipalFactory.GetUserId(User);
                var sellerId = _repository.Seller.GetSellerIdByUserId(userId);

                var product = _repository.CustomerOrderProduct.FindByCondition(c => c.Id == customerOrderProductId)
                    .FirstOrDefault();
                if (product == null)
                {
                    throw new BusinessException(XError.GetDataErrors.NotFound());
                }

                if (product.FinalStatusId != 23)
                {
                    throw new BusinessException(XError.BusinessErrors.InvalidOrderProductStatus());
                }

                product.FinalStatusId = 28;
                product.Mdate = DateTime.Now.Ticks;
                product.MuserId = userId;
                _repository.CustomerOrderProduct.Update(product);
                _repository.Save();

                _logger.LogData(MethodBase.GetCurrentMethod(), General.Results_.SuccessMessage(), null, customerOrderProductId);
                return Ok(General.Results_.SuccessMessage());
            }
            catch (Exception e)
            {
                _logger.LogError(e, MethodBase.GetCurrentMethod(), customerOrderProductId);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// لیست فروشندها براساس دسته بندی محصولات  
        /// </summary>

        [HttpGet]
        [Route("Seller/GetSellerListByCatProduct")]
        public IActionResult GetSellerListByCatProduct(List<long> catProductsIds)
        {
            try
            {
                if (catProductsIds.Count == 0)
                {

                    var res = _repository.Seller.FindByCondition(c => c.DaDate == null && c.Ddate == null)
                        .Select(c => new { c.Id, c.Name, c.Fname, c.SellerCode }).ToList();
                    _logger.LogData(MethodBase.GetCurrentMethod(), res, null, catProductsIds);
                    return Ok(res);
                }

                var ress = _repository.SellerCatProduct.FindByCondition(c => catProductsIds.Contains(c.CatProductId.Value))
                    .Include(c => c.Seller)
                    .Select(c => new { c.Seller.Id, c.Seller.Name, c.Seller.Fname, c.Seller.SellerCode }).ToList();
                _logger.LogData(MethodBase.GetCurrentMethod(), ress, null, catProductsIds);
                return Ok(ress);


            }
            catch (Exception e)
            {
                _logger.LogError(e, MethodBase.GetCurrentMethod(), catProductsIds);
                return BadRequest(e.Message);
            }
        }


        #region UI_Methods

        /// <summary>
        /// دریافت مشخصات صنعتگر  
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("Seller/GetSellerFullInfo")]
        public SingleResult<SellerFullInfoDto> GetSellerFullInfo_UI()
        {

            try
            {
                var userId = ClaimPrincipalFactory.GetUserId(User);

                var res = _repository.Seller.FindByCondition(c => c.UserId == userId)
                    .Include(c => c.SellerAddress)
                    .Include(c => c.SellerDocument).ThenInclude(c => c.Document).FirstOrDefault();
                var result = _mapper.Map<SellerFullInfoDto>(res);

                var finalres = SingleResult<SellerFullInfoDto>.GetSuccessfulResult(result);
                _logger.LogData(MethodBase.GetCurrentMethod(), finalres, null);
                return finalres;
            }
            catch (Exception e)
            {
                _logger.LogError(e, MethodBase.GetCurrentMethod());
                return SingleResult<SellerFullInfoDto>.GetFailResult(e.Message);
            }


        }


        /// <summary>
        /// ثبت مشخصات صنعتگر  
        /// </summary>
        [Authorize]
        [HttpPost]
        [Route("Seller/UpdateSellerFullInfo")]
        public LongResult UpdateSellerFullInfo(SellerRegisterDto input)
        {
            var userId = ClaimPrincipalFactory.GetUserId(User);//10136
            var seller = _repository.Seller.FindByCondition(c => c.UserId == userId).FirstOrDefault();
            if (seller == null)
            {
                var ress = LongResult.GetFailResult("فروشنده پیدا نشد!");
                _logger.LogData(MethodBase.GetCurrentMethod(), ress, null, input);
                return ress;

            }

            if (input.Mobile == null)
            {
                var ress = LongResult.GetFailResult("شماره موبایل  وارد نشده است");
                _logger.LogData(MethodBase.GetCurrentMethod(), ress, null, input);
                return ress;
            }

            try
            {

                seller.Mobile = input.Mobile;
                seller.Email = input.Email;
                seller.Name = input.Name;
                seller.MelliCode = input.MelliCode;
                seller.Fname = input.Fname;
                seller.MobileAppTypeId = input.MobileAppTypeId;
                seller.HaveMobileApp = input.HaveMobileApp;
                seller.RealOrLegal = input.RealOrLegal;
                seller.SecondMobile = input.SecondMobile;
                seller.ShabaNo = input.ShabaNo;
                seller.Tel = input.Tel;
                seller.SellerCode = _repository.Seller.FindAll().Max(c => c.SellerCode) + 1;
                seller.Cdate = DateTime.Now.Ticks;
                seller.Bdate = DateTimeFunc.MiladiToTimeTick(input.Bdate);
                seller.FinalStatusId = 14;
                seller.MobileAppVersion = input.MobileAppVersion;
                seller.Gender = input.Gender;
                seller.IdentityNo = input.IdentityNo;

                if (!string.IsNullOrWhiteSpace(input.PassWord))
                {
                    var u = _repository.Users.FindByCondition(c => c.Id == userId).FirstOrDefault();
                    u.Hpassword = input.PassWord;
                    _repository.Users.Update(u);
                }

                if (input.Address.ID == 0)
                {

                    var sellerAddress = new SellerAddress
                    {
                        Address = input.Address.Address,
                        Fax = input.Address.Fax,
                        CityId = input.Address.CityId,
                        PostalCode = input.Address.PostalCode,
                        ProvinceId = input.Address.ProvinceId,
                        Tel = input.Address.Tel,
                        Titel = input.Address.Titel,
                        Xgps = input.Address.Xgps,
                        Ygps = input.Address.Ygps,
                        Cdate = DateTime.Now.Ticks,
                        SellerId = seller.Id

                    };
                    _repository.SellerAddress.Create(sellerAddress);
                    _repository.Seller.Update(seller);
                    _repository.Save();
                }
                else
                {
                    var sellerAddress = _repository.SellerAddress.FindByCondition(c => c.Id == input.Address.ID).FirstOrDefault();
                    if (sellerAddress != null)
                    {

                        sellerAddress.Address = input.Address.Address;
                        sellerAddress.Fax = input.Address.Fax;
                        sellerAddress.CityId = input.Address.CityId;
                        sellerAddress.PostalCode = input.Address.PostalCode;
                        sellerAddress.ProvinceId = input.Address.ProvinceId;
                        sellerAddress.Tel = input.Address.Tel;
                        sellerAddress.Titel = input.Address.Titel;
                        sellerAddress.Xgps = input.Address.Xgps;
                        sellerAddress.Ygps = input.Address.Ygps;
                        sellerAddress.Mdate = DateTime.Now.Ticks;
                        sellerAddress.MuserId = ClaimPrincipalFactory.GetUserId(User);
                        _repository.SellerAddress.Update(sellerAddress);


                    }
                    _repository.Seller.Update(seller);
                    _repository.Save();
                }

                var finalres = LongResult.GetSingleSuccessfulResult(seller.Id);
                _logger.LogData(MethodBase.GetCurrentMethod(), finalres, null, input);
                return finalres;


            }
            catch (Exception e)
            {
                _logger.LogError(e, MethodBase.GetCurrentMethod(), input);
                return LongResult.GetFailResult("خطا در سامانه");
            }


        }

        /// <summary>
        /// لیست مدارک صنعتگر  
        /// </summary>
        [Authorize]
        [HttpPost]
        [Route("Seller/GetSellerDocumentList")]
        public ListResult<SellerDocumentDto> GetSellerDocumentList()
        {
            try
            {
                var seller = _repository.Seller.FindByCondition(c => c.UserId == ClaimPrincipalFactory.GetUserId(User)).FirstOrDefault();
                if (seller == null)
                {
                    var ress = ListResult<SellerDocumentDto>.GetFailResult("فروشنده پیدا نشد!");
                    _logger.LogData(MethodBase.GetCurrentMethod(), ress, null);
                    return ress;
                }


                var res = _repository.SellerDocument.FindByCondition(c => c.SellerId == seller.Id).Include(c => c.Document)
                    .ToList();
                var result = _mapper.Map<List<SellerDocumentDto>>(res);

                var finalres = ListResult<SellerDocumentDto>.GetSuccessfulResult(result, result.Count);
                _logger.LogData(MethodBase.GetCurrentMethod(), finalres, null);
                return finalres;
            }
            catch (Exception e)
            {
                _logger.LogError(e, MethodBase.GetCurrentMethod());
                return ListResult<SellerDocumentDto>.GetFailResult(e.Message);
            }


        }

        /// <summary>
        /// لیست مدارک صنعتگر  ثبت نهایی
        /// </summary>
        [Authorize]
        [HttpPost]
        [Route("Seller/SellerRegisterConfirm_UI")]
        public VoidResult SellerRegisterConfirm_UI()
        {
            try
            {
                var seller = _repository.Seller.FindByCondition(c => c.UserId == ClaimPrincipalFactory.GetUserId(User)).FirstOrDefault();

                if (seller == null)
                {
                    var ress = VoidResult.GetFailResult("فروشنده پیدا نشد!");
                    _logger.LogData(MethodBase.GetCurrentMethod(), ress, null);
                    return ress;
                }

                var requiredDocumentList = _repository.Document.FindByCondition(c => c.CatDocument.Rkey == 1 && c.IsRequired == true && c.Ddate == null && c.DaDate == null).ToList();
                var SellerUploadedDocument = _repository.SellerDocument.FindByCondition(c => c.SellerId == seller.Id && !string.IsNullOrWhiteSpace(c.FileUrl) && c.Document.IsRequired == true).ToList();

                if (requiredDocumentList.Count != SellerUploadedDocument.Count)
                {

                    var ress = VoidResult.GetFailResult("تمامی مدارک بارگزاری نشده است!");
                    _logger.LogData(MethodBase.GetCurrentMethod(), ress, null);
                    return ress;
                }


                var finalres = VoidResult.GetSuccessResult();
                _logger.LogData(MethodBase.GetCurrentMethod(), finalres, null);
                return finalres;
            }
            catch (Exception e)
            {
                _logger.LogError(e, MethodBase.GetCurrentMethod());
                return VoidResult.GetFailResult(e.Message);
            }


        }

        #endregion
    }
}
