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
                var res = _repository.Seller.FindByCondition(c => c.DaUserId == null && c.DuserId == null)
                    .ToList();

                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest("");
            }
        }


        #region UI_Methods

        [HttpPost]
        [Route("Account/SellerRegister_UI")]
        public async Task<LongResult> SellerRegister_UI(SellerRegisterDto seller)
        {
            try
            {
                if (!ModelState.IsValid) return LongResult.GetFailResult("فیلدهای اجباری پر نشده اند"); 
                if (await _repository.Users.FindByCondition(c => c.Username == seller.Mobile.ToString()).AnyAsync())
                    return LongResult.GetFailResult("UserName Already Exits!");


                Users _user = new Users
                {

                    Hpassword = seller.Hpassword,
                    Email = seller.Email,
                    Mobile = seller.Mobile,
                    Username = seller.Mobile.ToString(),
                    Cdate = DateTime.Now.Ticks


                };
                _user.UserRole.Add(new UserRole { Role = 3, Cdate = DateTime.Now.Ticks });
                _user.Seller.Add(new Seller
                {
                    Email = seller.Email,
                    Mobile = seller.Mobile,
                    Cdate = DateTime.Now.Ticks,


                });
                _repository.Users.Create(_user);
                _repository.Save();

                return LongResult.GetSingleSuccessfulResult(_user.Seller.FirstOrDefault().Id);
            }
            catch (Exception e)
            {
                return LongResult.GetFailResult(e.Message);
            }
        }

        #endregion
    }
}
