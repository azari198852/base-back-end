using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.BusinessModel;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.UIResponse;
using HandCarftBaseServer.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandCarftBaseServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public SellerController(IMapper mapper, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;
        }


        [HttpGet]
        [Route("Seller/GetSellerList")]
        public IActionResult GetFamousCommentsList()
        {
            try
            {
                var res = _repository.Seller.FindByCondition(c => c.DaDate == null && c.Ddate == null)
                    .ToList();

                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest("");
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

                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest("");
            }
        }


        [Authorize]
        [HttpGet]
        [Route("Seller/GetSellerFullInfo")]
        public SingleResult<SellerFullInfoDto> GetSellerFullInfo()
        {

            try
            {
                var userId = ClaimPrincipalFactory.GetUserId(User);

                var res = _repository.Seller.FindByCondition(c => c.UserId == userId)
                    .Include(c => c.SellerAddress)
                    .Include(c => c.SellerDocument).ThenInclude(c => c.Document).FirstOrDefault();
                var result = _mapper.Map<SellerFullInfoDto>(res);
                return SingleResult<SellerFullInfoDto>.GetSuccessfulResult(result);

            }
            catch (Exception e)
            {
                return SingleResult<SellerFullInfoDto>.GetFailResult(e.Message);
            }


        }


        [HttpGet]
        [Route("Seller/GetSellerFullInfo_test")]
        public SingleResult<SellerFullInfoDto> GetSellerFullInfo_test(long userId)
        {

            try
            {
                
                var res = _repository.Seller.FindByCondition(c => c.UserId == userId)
                    .Include(c => c.SellerAddress)
                    .Include(c => c.SellerDocument).ThenInclude(c => c.Document).FirstOrDefault();
                var result = _mapper.Map<SellerFullInfoDto>(res);
                return SingleResult<SellerFullInfoDto>.GetSuccessfulResult(result);

            }
            catch (Exception e)
            {
                return SingleResult<SellerFullInfoDto>.GetFailResult(e.Message);
            }


        }


        /// <summary>
        /// ثبت نام صنعتگر به سیستم 
        /// </summary>
        [HttpPost]
        [Route("Seller/UpdateSellerFullInfo")]
        public LongResult UpdateSellerFullInfo(SellerRegisterDto input)
        {

            try
            {
                if (input.Mobile == null)
                {
                    return LongResult.GetFailResult("شماره موبایل  وارد نشده است");
                }

                var user = _repository.Users.FindByCondition(c => (c.Mobile == input.Mobile) && c.UserRole.All(x => x.Role != 3))
                    .FirstOrDefault();

                if (user == null)
                {
                    var _user = new Users
                    {
                        Email = input.Email,
                        Mobile = input.Mobile,
                        FullName = input.Name + " " + input.Fname,
                        Hpassword = input.PassWord,
                        Username = input.Mobile.ToString(),
                        Cdate = DateTime.Now.Ticks

                    };

                    var userRole = new UserRole
                    {
                        Role = 3,
                        Cdate = DateTime.Now.Ticks,

                    };

                    _user.UserRole.Add(userRole);

                    var seller = new Seller
                    {
                        Mobile = input.Mobile,
                        Email = input.Email,
                        Fname = input.Name,
                        MelliCode = input.MelliCode,
                        Name = input.Fname,
                        MobileAppTypeId = input.MobileAppTypeId,
                        HaveMobileApp = input.HaveMobileApp,
                        RealOrLegal = input.RealOrLegal,
                        SecondMobile = input.SecondMobile,
                        ShabaNo = input.ShabaNo,
                        Tel = input.Tel,
                        SellerCode = _repository.Seller.FindAll().Max(c => c.SellerCode) + 1,
                        Cdate = DateTime.Now.Ticks,
                        Bdate = DateTimeFunc.MiladiToTimeTick(input.Bdate),
                        FinalStatusId = 14,
                        MobileAppVersion = input.MobileAppVersion,
                        Gender = input.Gender,
                        IdentityNo = input.IdentityNo

                    };

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

                    };
                    seller.SellerAddress.Add(sellerAddress);
                    _user.Seller.Add(seller);
                    _repository.Users.Create(_user);
                    _repository.Save();
                    return LongResult.GetSingleSuccessfulResult(seller.Id);
                }

                return LongResult.GetFailResult("برای این شماره موبایل قبلا ثبت نام انجام شد است");
            }
            catch (Exception e)
            {
           
                return LongResult.GetFailResult("خطا در سامانه");
            }


        }

        #region UI_Methods



        #endregion
    }
}
