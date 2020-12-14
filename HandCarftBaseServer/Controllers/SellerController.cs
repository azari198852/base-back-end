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

        #region UI_Methods

        //[HttpPost]
        //[Route("Account/SellerRegister_UI")]
        //public async Task<LongResult> SellerRegister_UI(SellerRegisterDto seller)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid) return LongResult.GetFailResult("فیلدهای اجباری پر نشده اند");
        //        if (await _repository.Users.FindByCondition(c => c.Username == seller.Mobile.ToString()).AnyAsync())
        //            return LongResult.GetFailResult("UserName Already Exits!");


        //        Users _user = new Users
        //        {

        //            Hpassword = seller.Hpassword,
        //            Email = seller.Email,
        //            Mobile = seller.Mobile,
        //            Username = seller.Mobile.ToString(),
        //            Cdate = DateTime.Now.Ticks


        //        };
        //        _user.UserRole.Add(new UserRole { Role = 3, Cdate = DateTime.Now.Ticks });
        //        _user.Seller.Add(new Seller
        //        {
        //            Email = seller.Email,
        //            Mobile = seller.Mobile,
        //            Cdate = DateTime.Now.Ticks,


        //        });
        //        _repository.Users.Create(_user);
        //        _repository.Save();

        //        return LongResult.GetSingleSuccessfulResult(_user.Seller.FirstOrDefault().Id);
        //    }
        //    catch (Exception e)
        //    {
        //        return LongResult.GetFailResult(e.Message);
        //    }
        //}

        #endregion
    }
}
