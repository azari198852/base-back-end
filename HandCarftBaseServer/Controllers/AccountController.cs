using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.BusinessModel;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.UIResponse;
using HandCarftBaseServer.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace HandCarftBaseServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILogger<CatProductController> _logger;

        public AccountController(IConfiguration configuration, IMapper mapper, IRepositoryWrapper repository, ILogger<CatProductController> logger)
        {
            _configuration = configuration;
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }



        [HttpPost]
        [Route("Account/Login")]
        public async Task<IActionResult> Login(UserLoginModel userLogin)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = _repository.Users.FindByCondition(c => c.Username == userLogin.Username && c.Hpassword == userLogin.Password)
                .Include(c => c.UserRole)
                .FirstOrDefault();

            if (user == null) return BadRequest("Invalid credentials");
            {
                //create claims details based on the user information
                var roleId = _repository.Role.FindByCondition(c => c.Id == user.UserRole.FirstOrDefault().Role)
                    .Select(c => c.Id).FirstOrDefault();
                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("FullName", user.FullName),
                    new Claim("UserName", user.Username),
                    new Claim("Email", user.Email),
                    new Claim("role", roleId.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
                var res = new { token = new JwtSecurityTokenHandler().WriteToken(token), Fullname = user.FullName };
                return Ok(res);
            }

        }

        [HttpPost]
        [Route("Account/SellerLogin")]
        public async Task<IActionResult> SellerLogin(UserLoginModel userLogin)
        {
            try
            {

                if (!ModelState.IsValid) return BadRequest(ModelState);
                var user = _repository.Users.FindByCondition(c => c.Username == userLogin.Username && c.Hpassword == userLogin.Password)
                    .Include(c => c.UserRole)
                    .FirstOrDefault();

                if (user == null || user.UserRole.All(c => c.Role != 3)) return BadRequest("Invalid credentials");
                {
                    //create claims details based on the user information
                    var roleId = _repository.Role.FindByCondition(c => c.Id == user.UserRole.FirstOrDefault().Role)
                        .Select(c => c.Id).FirstOrDefault();
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("FullName", user.FullName??""),
                        new Claim("UserName", user.Username),
                        new Claim("Email", user.Email ?? ""),
                        new Claim("role", "3")
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
                    var res = new { token = new JwtSecurityTokenHandler().WriteToken(token), Fullname = user.FullName };
                    return Ok(res);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }

        }


        /// <summary>
        ///ورود / ثبت نام مشتری به سیستم 
        /// </summary>
        [HttpPost]
        [Route("Account/CustomerLoginRegister_UI")]
        public SingleResult<LoginRegisterDto> CustomerLogin_UI(string email, long? mobileNo)
        {

            try
            {
                if (mobileNo != null)
                {

                    var user = _repository.Users.FindByCondition(c => c.Mobile == mobileNo)
                        .Include(c => c.UserRole)
                        .FirstOrDefault();

                    if (user == null || user.UserRole.All(c => c.Role != 2))
                    {
                        var _random = new Random();
                        var code = _random.Next(1000, 9999);
                        var _user = new Users
                        {
                            Mobile = mobileNo,
                            Username = mobileNo.ToString(),
                            Cdate = DateTime.Now.Ticks,
                            Hpassword = code.ToString()

                        };
                        _user.UserRole.Add(new UserRole { Role = 2, Cdate = DateTime.Now.Ticks });
                        _user.Customer.Add(new Customer
                        {

                            Mobile = mobileNo,
                            Cdate = DateTime.Now.Ticks,

                        });
                        _user.UserActivation.Add(new UserActivation
                        {
                            SendedCode = code,
                            EndDateTime = DateTime.Now.AddMinutes(2).Ticks,
                            Cdate = DateTime.Now.Ticks

                        });
                        _repository.Users.Create(_user);
                        _repository.Save();

                        var sms = new SendSMS();
                        sms.SendLoginSms(mobileNo.Value, code);
                        var ress = new LoginRegisterDto { UserId = _user.Id, IsExist = false };
                        return SingleResult<LoginRegisterDto>.GetSuccessfulResult(ress);

                    }

                    var resss = new LoginRegisterDto { UserId = user.Id, IsExist = true };
                    return SingleResult<LoginRegisterDto>.GetSuccessfulResult(resss);

                }
                else if (email != null && !string.IsNullOrWhiteSpace(email))
                {
                    var user = _repository.Users.FindByCondition(c => c.Email == email)
                        .Include(c => c.UserRole)
                        .FirstOrDefault();

                    if (user == null || user.UserRole.All(c => c.Role != 2))
                    {
                        Random _random = new Random();
                        var code = _random.Next(1000, 9999);
                        Users _user = new Users
                        {
                            Email = email,
                            Username = email,
                            Cdate = DateTime.Now.Ticks,
                            Hpassword = code.ToString()

                        };
                        _user.UserRole.Add(new UserRole { Role = 2, Cdate = DateTime.Now.Ticks });
                        _user.Customer.Add(new Customer
                        {

                            Email = email,
                            Cdate = DateTime.Now.Ticks,

                        });
                        _user.UserActivation.Add(new UserActivation
                        {
                            SendedCode = code,
                            EndDateTime = DateTime.Now.AddMinutes(2).Ticks,
                            Cdate = DateTime.Now.Ticks

                        });
                        _repository.Users.Create(_user);
                        _repository.Save();

                        SendEmail em = new SendEmail();
                        em.SendLoginEmail(email, code);
                        var ress = new LoginRegisterDto { UserId = _user.Id, IsExist = false };
                        return SingleResult<LoginRegisterDto>.GetSuccessfulResult(ress);

                    }

                    var resss = new LoginRegisterDto { UserId = user.Id, IsExist = true };
                    return SingleResult<LoginRegisterDto>.GetSuccessfulResult(resss);



                }
                return SingleResult<LoginRegisterDto>.GetFailResult("شماره موبایل یا ایمیل را وارد کنید");
            }
            catch (Exception e)
            {
                return SingleResult<LoginRegisterDto>.GetFailResult("خطا در سامانه");
            }


        }

        [HttpPost]
        [Route("Account/CustomerLogin_ByActivationCode_UI")]
        public SingleResult<LoginResultDto> CustomerLogin_ByActivationCode_UI(long userId, int activationCode)
        {

            try
            {
                var time = DateTime.Now.Ticks;
                var activation = _repository.UserActivation.FindByCondition(c =>
                        c.UserId == userId && c.SendedCode == activationCode)
                    .FirstOrDefault();
                if (activation == null) return SingleResult<LoginResultDto>.GetFailResult("کد وارد شده صحیح نمی باشد");

                var user = _repository.Users.FindByCondition(c => c.Id == userId)
                    .Include(c => c.UserRole)
                    .FirstOrDefault();


                //create claims details based on the user information
                var roleId = _repository.Role.FindByCondition(c => c.Id == user.UserRole.FirstOrDefault().Role)
                    .Select(c => c.Id).FirstOrDefault();
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("FullName", user.FullName??""),
                    new Claim("UserName", user.Username),
                    new Claim("Email", user.Email ?? ""),
                    new Claim("role", roleId.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims,
                    expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
                var res = new LoginResultDto { Token = new JwtSecurityTokenHandler().WriteToken(token), Fullname = user.FullName };
                return SingleResult<LoginResultDto>.GetSuccessfulResult(res);

            }
            catch (Exception e)
            {
                return SingleResult<LoginResultDto>.GetFailResult(e.Message);
            }


        }

        [HttpPost]
        [Route("Account/CustomerLogin_ByPass_UI")]
        public SingleResult<LoginResultDto> CustomerLogin_ByPass_UI(long userId, string pass)
        {

            try
            {

                var user = _repository.Users.FindByCondition(c => c.Id == userId && c.Hpassword == pass)
                    .Include(c => c.UserRole)
                    .FirstOrDefault();
                if (user == null || user.UserRole.All(c => c.Role != 2)) return SingleResult<LoginResultDto>.GetFailResult("نام کاربری یا کلمه عبور صحیح نمی باشد.");

                //create claims details based on the user information
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("FullName", user.FullName??""),
                    new Claim("UserName", user.Username),
                    new Claim("Email", user.Email ?? ""),
                    new Claim("role", 2.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims,
                    expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
                var res = new LoginResultDto { Token = new JwtSecurityTokenHandler().WriteToken(token), Fullname = user.FullName };
                return SingleResult<LoginResultDto>.GetSuccessfulResult(res);

            }
            catch (Exception e)
            {
                return SingleResult<LoginResultDto>.GetFailResult(e.Message);
            }


        }

        ///// <summary>
        /////دریافت شماره موبایل برای بررسی موجود بودن در سامانه
        ///// </summary>
        //[HttpPost]
        //[Route("Account/CustomerRegister_UI")]
        //public LongResult CustomerRegister_UI(long mobileNo)
        //{
        //    try
        //    {

        //        if (_repository.Users.FindByCondition(c => c.Username == mobileNo.ToString()).Any())
        //            return LongResult.GetSingleSuccessfulResult(-1);

        //        Users _user = new Users
        //        {



        //            Mobile = mobileNo,
        //            Username = mobileNo.ToString(),
        //            Cdate = DateTime.Now.Ticks,

        //        };
        //        _user.UserRole.Add(new UserRole { Role = 2, Cdate = DateTime.Now.Ticks });
        //        _user.Customer.Add(new Customer
        //        {


        //            Mobile = mobileNo,
        //            Cdate = DateTime.Now.Ticks,


        //        });
        //        _repository.Users.Create(_user);
        //        _repository.Save();
        //        return LongResult.GetSingleSuccessfulResult(_user.Id);
        //    }
        //    catch (Exception e)
        //    {
        //        return LongResult.GetFailResult(null);
        //    }
        //}

        ///// <summary>
        /////دریافت پسورد جهت آپدیت و تکمیال ثبت نام
        ///// </summary>
        //[HttpPost]
        //[Route("Account/CustomerRegister_GetPassword_UI")]
        //public LongResult CustomerRegister_GetPassword_UI(long userId, long mobileNo, string password)
        //{
        //    try
        //    {

        //        var _user = _repository.Users.FindByCondition(c => c.Id == userId && c.Username == mobileNo.ToString()).FirstOrDefault();
        //        if (_user == null)
        //            return LongResult.GetFailResult("اطلاعات ورودی صحیح نمی باشد");

        //        _user.Hpassword = password;
        //        _repository.Users.Update(_user);
        //        _repository.Save();
        //        return LongResult.GetSingleSuccessfulResult(_user.Id);
        //    }
        //    catch (Exception e)
        //    {
        //        return LongResult.GetFailResult(null);
        //    }
        //}


        /// <summary>
        ///ورود / ثبت نام صنعتگر به سیستم 
        /// </summary>
        [HttpPost]
        [Route("Account/SellerLoginRegister_UI")]
        public SingleResult<LoginRegisterDto> SellerLoginRegister_UI(string email, long? mobileNo)
        {

            try
            {
                if (mobileNo != null)
                {

                    var user = _repository.Users.FindByCondition(c => c.Mobile == mobileNo)
                        .Include(c => c.UserRole)
                        .FirstOrDefault();

                    if (user == null || user.UserRole.All(c => c.Role != 3))
                    {
                        var _random = new Random();
                        var code = _random.Next(1000, 9999);
                        var _user = new Users
                        {
                            Mobile = mobileNo,
                            Username = mobileNo.ToString(),
                            Cdate = DateTime.Now.Ticks,
                            Hpassword = code.ToString()

                        };
                        _user.UserRole.Add(new UserRole { Role = 3, Cdate = DateTime.Now.Ticks });
                        _user.Seller.Add(new Seller
                        {

                            Mobile = mobileNo,
                            Cdate = DateTime.Now.Ticks,

                        });
                        _user.UserActivation.Add(new UserActivation
                        {
                            SendedCode = code,
                            EndDateTime = DateTime.Now.AddMinutes(2).Ticks,
                            Cdate = DateTime.Now.Ticks

                        });
                        _repository.Users.Create(_user);
                        _repository.Save();

                        var sms = new SendSMS();
                        sms.SendLoginSms(mobileNo.Value, code);
                        var ress = new LoginRegisterDto { UserId = _user.Id, IsExist = false };
                        return SingleResult<LoginRegisterDto>.GetSuccessfulResult(ress);

                    }

                    var resss = new LoginRegisterDto { UserId = user.Id, IsExist = true };
                    return SingleResult<LoginRegisterDto>.GetSuccessfulResult(resss);

                }
                else if (email != null && !string.IsNullOrWhiteSpace(email))
                {
                    var user = _repository.Users.FindByCondition(c => c.Email == email)
                        .Include(c => c.UserRole)
                        .FirstOrDefault();

                    if (user == null || user.UserRole.All(c => c.Role != 3))
                    {
                        Random _random = new Random();
                        var code = _random.Next(1000, 9999);
                        Users _user = new Users
                        {
                            Email = email,
                            Username = email,
                            Cdate = DateTime.Now.Ticks,
                            Hpassword = code.ToString()

                        };
                        _user.UserRole.Add(new UserRole { Role = 3, Cdate = DateTime.Now.Ticks });
                        _user.Seller.Add(new Seller
                        {

                            Email = email,
                            Cdate = DateTime.Now.Ticks,

                        });
                        _user.UserActivation.Add(new UserActivation
                        {
                            SendedCode = code,
                            EndDateTime = DateTime.Now.AddMinutes(2).Ticks,
                            Cdate = DateTime.Now.Ticks

                        });
                        _repository.Users.Create(_user);
                        _repository.Save();

                        SendEmail em = new SendEmail();
                        em.SendLoginEmail(email, code);
                        var ress = new LoginRegisterDto { UserId = _user.Id, IsExist = false };
                        return SingleResult<LoginRegisterDto>.GetSuccessfulResult(ress);

                    }

                    var resss = new LoginRegisterDto { UserId = user.Id, IsExist = true };
                    return SingleResult<LoginRegisterDto>.GetSuccessfulResult(resss);



                }
                return SingleResult<LoginRegisterDto>.GetFailResult("شماره موبایل یا ایمیل را وارد کنید");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return SingleResult<LoginRegisterDto>.GetFailResult("خطا در سامانه");
            }


        }

        [HttpPost]
        [Route("Account/SellerLogin_ByActivationCode_UI")]
        public SingleResult<LoginResultDto> SellerLogin_ByActivationCode_UI(long userId, int activationCode)
        {

            try
            {
                var time = DateTime.Now.Ticks;
                var activation = _repository.UserActivation.FindByCondition(c =>
                        c.UserId == userId && c.SendedCode == activationCode)
                    .FirstOrDefault();
                if (activation == null) return SingleResult<LoginResultDto>.GetFailResult("کد وارد شده صحیح نمی باشد");

                var user = _repository.Users.FindByCondition(c => c.Id == userId)
                    .Include(c => c.UserRole)
                    .FirstOrDefault();


                //create claims details based on the user information
                var roleId = _repository.Role.FindByCondition(c => c.Id == user.UserRole.FirstOrDefault().Role)
                    .Select(c => c.Id).FirstOrDefault();
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("FullName", user.FullName??""),
                    new Claim("UserName", user.Username),
                    new Claim("Email", user.Email ?? ""),
                    new Claim("role", "3")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims,
                    expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
                var res = new LoginResultDto { Token = new JwtSecurityTokenHandler().WriteToken(token), Fullname = user.FullName };
                return SingleResult<LoginResultDto>.GetSuccessfulResult(res);

            }
            catch (Exception e)
            {
                return SingleResult<LoginResultDto>.GetFailResult(e.Message);
            }


        }

        [HttpPost]
        [Route("Account/SellerLogin_ByPass_UI")]
        public SingleResult<LoginResultDto> SellerLogin_ByPass_UI(long userId, string pass)
        {

            try
            {

                var user = _repository.Users.FindByCondition(c => c.Id == userId && c.Hpassword == pass)
                    .Include(c => c.UserRole)
                    .FirstOrDefault();
                if (user == null || user.UserRole.All(c => c.Role != 3)) return SingleResult<LoginResultDto>.GetFailResult("نام کاربری یا کلمه عبور صحیح نمی باشد.");

                //create claims details based on the user information
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("FullName", user.FullName??""),
                    new Claim("UserName", user.Username),
                    new Claim("Email", user.Email ?? ""),
                    new Claim("role", "3")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims,
                    expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
                var res = new LoginResultDto { Token = new JwtSecurityTokenHandler().WriteToken(token), Fullname = user.FullName };
                return SingleResult<LoginResultDto>.GetSuccessfulResult(res);

            }
            catch (Exception e)
            {
                return SingleResult<LoginResultDto>.GetFailResult(e.Message);
            }


        }


    }
}

