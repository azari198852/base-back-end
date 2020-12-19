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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Bcpg;

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



        #region UI_Methods

        /// <summary>
        ///ورود / ثبت نام مشتری به سیستم 
        /// </summary>
        [HttpPost]
        [Route("Account/CustomerLoginRegister_UI")]
        public SingleResult<LoginRegisterDto> CustomerLogin_UI(string email, long? mobileNo)
        {

            try
            {
                var _random = new Random();
                var code = _random.Next(1000, 9999);
                if (mobileNo != null)
                {

                    var user = _repository.Users.FindByCondition(c => c.Mobile == mobileNo)
                        .Include(c => c.UserRole)
                        .FirstOrDefault();

                    if (user == null)
                    {

                        var _user = new Users
                        {
                            Mobile = mobileNo,
                            Username = mobileNo.ToString(),
                            Cdate = DateTime.Now.Ticks,

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
                            Cdate = DateTime.Now.Ticks,
                            LoginType = 1

                        });
                        _repository.Users.Create(_user);

                        _repository.Save();

                        var sms = new SendSMS();
                        sms.SendLoginSms(mobileNo.Value, code);
                        var ress = new LoginRegisterDto { UserId = _user.Id, IsExist = false, LoginByCode = true };
                        return SingleResult<LoginRegisterDto>.GetSuccessfulResult(ress);

                    }
                    else
                    {

                        if (user.UserRole.All(c => c.Role != 2))
                        {

                            _repository.UserRole.Create(new UserRole { UserId = user.Id, Role = 2, Cdate = DateTime.Now.Ticks });
                            var customer = new Customer
                            {
                                Email = user.Email,
                                Fname = user.FullName,
                                Mobile = user.Mobile,
                                Name = user.FullName,
                                UserId = user.Id
                                

                            };
                            _repository.Customer.Create(customer);

                        }

                        if (string.IsNullOrWhiteSpace(user.Hpassword))
                        {


                            _repository.UserActivation.Create(new UserActivation
                            {
                                SendedCode = code,
                                EndDateTime = DateTime.Now.AddMinutes(2).Ticks,
                                Cdate = DateTime.Now.Ticks,
                                LoginType = 1,
                                UserId = user.Id


                            });
                           
                         
                            _repository.Save();

                            var sms = new SendSMS();
                            sms.SendLoginSms(mobileNo.Value, code);
                            var ressss = new LoginRegisterDto { UserId = user.Id, IsExist = true, LoginByCode = true };
                            return SingleResult<LoginRegisterDto>.GetSuccessfulResult(ressss);
                        }
                        else
                        {

                            _repository.Save();
                            var ressss = new LoginRegisterDto { UserId = user.Id, IsExist = true, LoginByCode = false };
                            return SingleResult<LoginRegisterDto>.GetSuccessfulResult(ressss);


                        }


                    }


                }
                else if (email != null && !string.IsNullOrWhiteSpace(email))
                {
                    var user = _repository.Users.FindByCondition(c => c.Email == email)
                        .Include(c => c.UserRole)
                        .FirstOrDefault();

                    if (user == null)
                    {

                        Users _user = new Users
                        {
                            Email = email,
                            Username = email,
                            Cdate = DateTime.Now.Ticks

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
                            Cdate = DateTime.Now.Ticks,
                            LoginType = 1

                        });
                        _repository.Users.Create(_user);
                        _repository.Save();

                        SendEmail em = new SendEmail();
                        em.SendLoginEmail(email, code);
                        var ress = new LoginRegisterDto { UserId = _user.Id, IsExist = false, LoginByCode = true };
                        return SingleResult<LoginRegisterDto>.GetSuccessfulResult(ress);

                    }
                    else
                    {

                        if (user.UserRole.All(c => c.Role != 2))
                        {

                            _repository.UserRole.Create(new UserRole { UserId = user.Id, Role = 2, Cdate = DateTime.Now.Ticks });
                            var customer = new Customer
                            {
                                Email = user.Email,
                                Fname = user.FullName,
                                Mobile = user.Mobile,
                                Name = user.FullName,
                                UserId = user.Id


                            };
                            _repository.Customer.Create(customer);


                        }

                        if (string.IsNullOrWhiteSpace(user.Hpassword))
                        {


                            _repository.UserActivation.Create(new UserActivation
                            {
                                SendedCode = code,
                                EndDateTime = DateTime.Now.AddMinutes(2).Ticks,
                                Cdate = DateTime.Now.Ticks,
                                LoginType = 1,
                                UserId = user.Id

                            });
                            _repository.Save();

                            var em = new SendEmail();
                            em.SendLoginEmail(email, code);
                            var ressss = new LoginRegisterDto { UserId = user.Id, IsExist = true, LoginByCode = true };
                            return SingleResult<LoginRegisterDto>.GetSuccessfulResult(ressss);
                        }
                        else
                        {

                            _repository.Save();
                            var ressss = new LoginRegisterDto { UserId = user.Id, IsExist = true, LoginByCode = false };
                            return SingleResult<LoginRegisterDto>.GetSuccessfulResult(ressss);


                        }


                    }

                }
                return SingleResult<LoginRegisterDto>.GetFailResult("شماره موبایل یا ایمیل را وارد کنید");
            }
            catch (Exception e)
            {
                return SingleResult<LoginRegisterDto>.GetFailResult("خطا در سامانه");
            }


        }

        /// <summary>
        ///ورود مشتری به سیستم با کد  
        /// </summary>
        [HttpPost]
        [Route("Account/CustomerLogin_ByActivationCode_UI")]
        public SingleResult<LoginResultDto> CustomerLogin_ByActivationCode_UI(long userId, int activationCode)
        {

            try
            {
                var time = DateTime.Now.Ticks;
                var activation = _repository.UserActivation.FindByCondition(c =>
                        c.UserId == userId && c.SendedCode == activationCode && c.EndDateTime > time && c.LoginType == 1)
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
                    new Claim("Mobile", user.Mobile.ToString()),
                    new Claim("Email", user.Email ?? ""),
                    new Claim("role", roleId.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims,
                    expires: DateTime.UtcNow.AddHours(1), signingCredentials: signIn);
                var res = new LoginResultDto { Token = new JwtSecurityTokenHandler().WriteToken(token), Fullname = user.FullName };
                return SingleResult<LoginResultDto>.GetSuccessfulResult(res);

            }
            catch (Exception e)
            {
                return SingleResult<LoginResultDto>.GetFailResult(e.Message);
            }


        }

        /// <summary>
        ///ورود مشتری به سیستم با کلمه عبور  
        /// </summary>
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
                    new Claim("Mobile", user.Mobile.ToString()),
                    new Claim("Email", user.Email ?? ""),
                    new Claim("role", 2.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims,
                    expires: DateTime.UtcNow.AddHours(1), signingCredentials: signIn);
                var res = new LoginResultDto { Token = new JwtSecurityTokenHandler().WriteToken(token), Fullname = user.FullName };
                return SingleResult<LoginResultDto>.GetSuccessfulResult(res);

            }
            catch (Exception e)
            {
                return SingleResult<LoginResultDto>.GetFailResult(e.Message);
            }


        }

        /// <summary>
        ///درخواست کد فعالسازی جهت تغییر کلمه عبور  
        /// </summary>
        [HttpGet]
        [Route("Account/Customer_GetCodeForForgetPass")]
        public VoidResult CustomerLogin_ForgetPass(string email, long? mobileNo)
        {

            try
            {

                var user = _repository.Users.FindByCondition(c => (c.Mobile == mobileNo && mobileNo != null) || (c.Email == email && email != null)).Include(c => c.UserRole).FirstOrDefault();
                if (user == null || user.UserRole.All(c => c.Role != 2)) return VoidResult.GetFailResult("کاربری با مشخصات وارد شده یافت نشد.");
                var now = DateTime.Now.Ticks;
                if (_repository.UserActivation.FindByCondition(c => c.UserId == user.Id && c.EndDateTime > now && c.LoginType == 2).Any())
                    return VoidResult.GetFailResult("کد فعالسازی قبلا برای شما ارسال گردیده است.");

                var random = new Random();
                var code = random.Next(1000, 9999);


                if (mobileNo != null)
                {

                    user.UserActivation.Add(new UserActivation
                    {
                        SendedCode = code,
                        EndDateTime = DateTime.Now.AddMinutes(2).Ticks,
                        Cdate = DateTime.Now.Ticks,
                        LoginType = 2,
                        UserId = user.Id

                    });

                    var sms = new SendSMS();
                    sms.SendRestPassSms(mobileNo.Value, code);

                }
                else
                {
                    user.UserActivation.Add(new UserActivation
                    {
                        SendedCode = code,
                        EndDateTime = DateTime.Now.AddMinutes(2).Ticks,
                        Cdate = DateTime.Now.Ticks,
                        LoginType = 2,
                        UserId = user.Id

                    });
                    SendEmail em = new SendEmail();
                    em.SendResetPassEmail(email, code);

                }
                _repository.Users.Update(user);
                _repository.Save();
                //create claims details based on the user information

                return VoidResult.GetSuccessResult("کد فعال سازی برای بازیابی رمز عبور ، با موفقیت ارسال شد");

            }
            catch (Exception e)
            {
                return VoidResult.GetFailResult(e.Message);
            }


        }

        /// <summary>
        ///تغییر کلمه عبور با کد تایید دریافتی با استفاده از موبایل یا ایمیل 
        /// </summary>
        [HttpGet]
        [Route("Account/Customer_ChangePassByActivationCode")]
        public VoidResult Customer_ChangePassByActivationCode(string email, long? mobileNo, int code, string pass)
        {

            try
            {

                var user = _repository.Users.FindByCondition(c => (c.Mobile == mobileNo && mobileNo != null) || (c.Email == email && email != null)).Include(c => c.UserRole).FirstOrDefault();
                if (user == null || user.UserRole.All(c => c.Role != 2)) return VoidResult.GetFailResult("کاربری با مشخصات وارد شده یافت نشد.");
                var now = DateTime.Now.Ticks;
                var s = _repository.UserActivation.FindByCondition(c =>
                    c.UserId == user.Id && c.EndDateTime > now && c.SendedCode == code).FirstOrDefault();
                if (s == null) return VoidResult.GetFailResult("کد وارد شده جهت تغییر کلمه عبور صحیح نمی باشد.");

                user.Hpassword = pass;
                _repository.Users.Update(user);
                _repository.Save();

                return VoidResult.GetSuccessResult("زمز عبور با نوفقیت تغییر یافت .");

            }
            catch (Exception e)
            {
                return VoidResult.GetFailResult(e.Message);
            }


        }

        /// <summary>
        ///درخواست کد فعالسازی یرای ورود به سیستم 
        /// </summary>
        [HttpGet]
        [Route("Account/Customer_GetActivationCodeForLogin")]
        public VoidResult Customer_ChangePassByActivationCode(string email, long? mobileNo, long? userId)
        {

            try
            {

                var user = _repository.Users.FindByCondition(c => (c.Mobile == mobileNo && mobileNo != null) || (c.Email == email && email != null) || (c.Id == userId && userId != null)).Include(c => c.UserRole).FirstOrDefault();
                if (user == null || user.UserRole.All(c => c.Role != 2)) return VoidResult.GetFailResult("کاربری با مشخصات وارد شده یافت نشد.");
                var now = DateTime.Now.Ticks;
                if (_repository.UserActivation.FindByCondition(c => (c.UserId == user.Id) && (c.EndDateTime > now) && (c.LoginType == 1)).Any())
                    return VoidResult.GetFailResult("کد فعالسازی قبلا برای شما ارسال گردیده است.");

                var _random = new Random();
                var code = _random.Next(1000, 9999);

                user.UserActivation.Add(new UserActivation
                {
                    SendedCode = code,
                    EndDateTime = DateTime.Now.AddMinutes(2).Ticks,
                    Cdate = DateTime.Now.Ticks,
                    LoginType = 1

                });
                _repository.Users.Update(user);
                _repository.Save();

                if (!string.IsNullOrWhiteSpace(email) || !string.IsNullOrWhiteSpace(user.Email))
                {

                    var em = new SendEmail();
                    em.SendLoginEmail(email ?? user.Email, code);
                }

                if (mobileNo != null || user.Mobile != null)
                {
                    var sms = new SendSMS();
                    sms.SendLoginSms(mobileNo ?? user.Mobile.Value, code);

                }



                return VoidResult.GetSuccessResult("کد فعالسازی با موفقیت ارسال گردید .");

            }
            catch (Exception e)
            {
                return VoidResult.GetFailResult(e.Message);
            }


        }

        [Authorize]
        [HttpPut]
        [Route("Account/Customer_UpdateProfileInfo")]
        public VoidResult Customer_UpdateProfileInfo(CustomerProfileDto customerProfileDto)
        {

            try
            {
                var userId = ClaimPrincipalFactory.GetUserId(User);
                var user = _repository.Users.FindByCondition(c => c.Id == userId).Include(c => c.UserRole).FirstOrDefault();
                if (user == null || user.UserRole.All(c => c.Role != 2)) return VoidResult.GetFailResult("کاربری با مشخصات وارد شده یافت نشد.");

                user.FullName = customerProfileDto.Name + " " + customerProfileDto.Fname;
                user.Email = customerProfileDto.Email;
                user.Hpassword = customerProfileDto.Password;
                user.Mobile = customerProfileDto.Mobile;

                _repository.Users.Update(user);

                var customer = _repository.Customer.FindByCondition(c => c.UserId == userId).FirstOrDefault();
                customer.Name = customerProfileDto.Name;
                customer.Fname = customerProfileDto.Fname;
                customer.MelliCode = customerProfileDto.MelliCode;
                customer.Bdate = customerProfileDto.Bdate?.Ticks;
                customer.Mobile = customerProfileDto.Mobile;
                customer.Email = customerProfileDto.Email;
                customer.WorkId = customerProfileDto.WorkId;
                _repository.Customer.Update(customer);

                _repository.Save();

                return VoidResult.GetSuccessResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return VoidResult.GetFailResult(e.Message);
            }


        }

        //[Authorize]
        [HttpGet]
        [Route("Account/Customer_GetProfileInfo")]
        public SingleResult<CustomerProfileDto> Customer_GetProfileInfo()
        {
            var userId = ClaimPrincipalFactory.GetUserId(User);
            try
            {

                var customer = _repository.Customer.FindByCondition(c => c.UserId == userId).Include(c => c.Work).FirstOrDefault();
                if (customer == null)
                    return SingleResult<CustomerProfileDto>.GetFailResult("مشتری یافت نشد!", 201);
                var cust = _mapper.Map<CustomerProfileDto>(customer);

                cust.Password = null;

                return SingleResult<CustomerProfileDto>.GetSuccessfulResult(cust);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message, userId);
                return SingleResult<CustomerProfileDto>.GetFailResult(e.Message);
            }


        }

        #endregion

        /// <summary>
        ///ورود / ثبت نام صنعتگر به سیستم 
        /// </summary>
        [HttpPost]
        [Route("Account/SellerLoginRegister")]
        public SingleResult<LoginRegisterDto> SellerLoginRegister(long mobileNo)
        {

            try
            {

                var user = _repository.Users.FindByCondition(c => c.Mobile == mobileNo).Include(c => c.UserRole)
                    .FirstOrDefault();

                var _random = new Random();
                var code = _random.Next(1000, 9999);

                if (user == null)
                {
                    var _user = new Users
                    {
                        Mobile = mobileNo,
                        Username = mobileNo.ToString(),
                        Cdate = DateTime.Now.Ticks,

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
                        Cdate = DateTime.Now.Ticks,
                        LoginType = 1

                    });
                    _repository.Users.Create(_user);
                    _repository.Save();

                    var sms = new SendSMS();
                    sms.SendLoginSms(mobileNo, code);
                    var ress = new LoginRegisterDto { UserId = _user.Id, IsExist = false, LoginByCode = true };
                    return SingleResult<LoginRegisterDto>.GetSuccessfulResult(ress);

                }
                else
                {
                    if (user.UserRole.All(c => c.Role != 3))
                    {

                        _repository.UserRole.Create(new UserRole { UserId = user.Id, Role = 3, Cdate = DateTime.Now.Ticks });
                        var seller = new Seller
                        {

                            UserId = user.Id,
                            Mobile = mobileNo,
                            Cdate = DateTime.Now.Ticks,
                            Name = user.FullName

                        };
                        _repository.Seller.Create(seller);

                    }

                    if (string.IsNullOrWhiteSpace(user.Hpassword))
                    {


                        _repository.UserActivation.Create(new UserActivation
                        {
                            SendedCode = code,
                            EndDateTime = DateTime.Now.AddMinutes(2).Ticks,
                            Cdate = DateTime.Now.Ticks,
                            LoginType = 1,
                            UserId = user.Id

                        });
                        _repository.Save();

                        var sms = new SendSMS();
                        sms.SendLoginSms(mobileNo, code);
                        var ressss = new LoginRegisterDto { UserId = user.Id, IsExist = true, LoginByCode = true };
                        return SingleResult<LoginRegisterDto>.GetSuccessfulResult(ressss);
                    }
                    else
                    {

                        _repository.Save();
                        var ressss = new LoginRegisterDto { UserId = user.Id, IsExist = true, LoginByCode = false };
                        return SingleResult<LoginRegisterDto>.GetSuccessfulResult(ressss);


                    }


                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return SingleResult<LoginRegisterDto>.GetFailResult("خطا در سامانه");
            }


        }

        /// <summary>
        ///ورود صنعتگر به سیستم با کد  
        /// </summary>
        [HttpPost]
        [Route("Account/SellerLogin_ByActivationCode_UI")]
        public SingleResult<LoginResultDto> SellerLogin_ByActivationCode_UI(long userId, int activationCode)
        {

            try
            {
                var time = DateTime.Now.Ticks;
                var activation = _repository.UserActivation.FindByCondition(c =>
                        c.UserId == userId && c.SendedCode == activationCode && c.EndDateTime > time && c.LoginType == 1)
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
                    new Claim("Mobile", user.Mobile.ToString()),
                    new Claim("Email", user.Email ?? ""),
                    new Claim("role","3")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims,
                    expires: DateTime.UtcNow.AddHours(1), signingCredentials: signIn);
                var res = new LoginResultDto { Token = new JwtSecurityTokenHandler().WriteToken(token), Fullname = user.FullName };
                return SingleResult<LoginResultDto>.GetSuccessfulResult(res);

            }
            catch (Exception e)
            {
                return SingleResult<LoginResultDto>.GetFailResult(e.Message);
            }


        }

        /// <summary>
        ///ورود صنعتگر به سیستم با کلمه عبور  
        /// </summary>
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
                    new Claim("Mobile", user.Mobile.ToString()),
                    new Claim("Email", user.Email ?? ""),
                    new Claim("role", 3.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims,
                    expires: DateTime.UtcNow.AddHours(1), signingCredentials: signIn);
                var res = new LoginResultDto { Token = new JwtSecurityTokenHandler().WriteToken(token), Fullname = user.FullName };
                return SingleResult<LoginResultDto>.GetSuccessfulResult(res);

            }
            catch (Exception e)
            {
                return SingleResult<LoginResultDto>.GetFailResult(e.Message);
            }


        }

        /// <summary>
        ///درخواست کد فعالسازی جهت تغییر کلمه عبور صنعتگر  
        /// </summary>
        [HttpGet]
        [Route("Account/SellerLogin_ForgetPass")]
        public VoidResult SellerLogin_ForgetPass(string email, long? mobileNo)
        {

            try
            {

                var user = _repository.Users.FindByCondition(c => (c.Mobile == mobileNo && mobileNo != null) || (c.Email == email && email != null)).Include(c => c.UserRole).FirstOrDefault();
                if (user == null || user.UserRole.All(c => c.Role != 3)) return VoidResult.GetFailResult("کاربری با مشخصات وارد شده یافت نشد.");
                var now = DateTime.Now.Ticks;
                if (_repository.UserActivation.FindByCondition(c => c.UserId == user.Id && c.EndDateTime > now && c.LoginType == 2).Any())
                    return VoidResult.GetFailResult("کد فعالسازی قبلا برای شما ارسال گردیده است.");

                var random = new Random();
                var code = random.Next(1000, 9999);


                if (mobileNo != null)
                {

                    user.UserActivation.Add(new UserActivation
                    {
                        SendedCode = code,
                        EndDateTime = DateTime.Now.AddMinutes(2).Ticks,
                        Cdate = DateTime.Now.Ticks,
                        LoginType = 2,
                        UserId = user.Id


                    });

                    var sms = new SendSMS();
                    var bb = sms.SendRestPassSms(mobileNo.Value, code);
                    _logger.LogInformation(bb);

                }
                else
                {
                    user.UserActivation.Add(new UserActivation
                    {
                        SendedCode = code,
                        EndDateTime = DateTime.Now.AddMinutes(2).Ticks,
                        Cdate = DateTime.Now.Ticks,
                        LoginType = 2,
                        UserId = user.Id

                    });
                    SendEmail em = new SendEmail();
                    em.SendResetPassEmail(email, code);

                }
                _repository.Users.Update(user);
                _repository.Save();
                //create claims details based on the user information

                return VoidResult.GetSuccessResult("کد فعال سازی برای بازیابی رمز عبور ، با موفقیت ارسال شد");

            }
            catch (Exception e)
            {
                return VoidResult.GetFailResult(e.Message);
            }


        }

        /// <summary>
        ///تغییر کلمه عبور با کد تایید دریافتی با استفاده از موبایل یا ایمیل صنعتگر  
        /// </summary>
        [HttpGet]
        [Route("Account/Seller_ChangePassByActivationCode")]
        public VoidResult Seller_ChangePassByActivationCode(string email, long? mobileNo, int code, string pass)
        {

            try
            {

                var user = _repository.Users.FindByCondition(c => (c.Mobile == mobileNo && mobileNo != null) || (c.Email == email && email != null)).Include(c => c.UserRole).FirstOrDefault();
                if (user == null || user.UserRole.All(c => c.Role != 3)) return VoidResult.GetFailResult("کاربری با مشخصات وارد شده یافت نشد.");
                var now = DateTime.Now.Ticks;
                var s = _repository.UserActivation.FindByCondition(c =>
                    c.UserId == user.Id && c.EndDateTime > now && c.SendedCode == code).FirstOrDefault();
                if (s == null) return VoidResult.GetFailResult("کد وارد شده جهت تغییر کلمه عبور صحیح نمی باشد.");

                user.Hpassword = pass;
                _repository.Users.Update(user);
                _repository.Save();

                return VoidResult.GetSuccessResult("زمز عبور با نوفقیت تغییر یافت .");

            }
            catch (Exception e)
            {
                return VoidResult.GetFailResult(e.Message);
            }


        }

        /// <summary>
        ///درخواست کد فعالسازی یرای ورود به سیستم صنعتگر 
        /// </summary>
        [HttpGet]
        [Route("Account/Seller_GetActivationCodeForLogin")]
        public VoidResult Seller_ChangePassByActivationCode(string email, long? mobileNo, long? userId)
        {

            try
            {

                var user = _repository.Users.FindByCondition(c => (c.Mobile == mobileNo && mobileNo != null) || (c.Email == email && email != null) || (c.Id == userId && userId != null)).Include(c => c.UserRole).FirstOrDefault();
                if (user == null || user.UserRole.All(c => c.Role != 3)) return VoidResult.GetFailResult("کاربری با مشخصات وارد شده یافت نشد.");
                var now = DateTime.Now.Ticks;
                if (_repository.UserActivation.FindByCondition(c => (c.UserId == user.Id) && (c.EndDateTime > now) && (c.LoginType == 1)).Any())
                    return VoidResult.GetFailResult("کد فعالسازی قبلا برای شما ارسال گردیده است.");

                var _random = new Random();
                var code = _random.Next(1000, 9999);

                user.UserActivation.Add(new UserActivation
                {
                    SendedCode = code,
                    EndDateTime = DateTime.Now.AddMinutes(2).Ticks,
                    Cdate = DateTime.Now.Ticks,
                    LoginType = 1

                });
                _repository.Users.Update(user);
                _repository.Save();

                if (!string.IsNullOrWhiteSpace(email) || !string.IsNullOrWhiteSpace(user.Email))
                {

                    var em = new SendEmail();
                    em.SendLoginEmail(email ?? user.Email, code);
                }

                if (mobileNo != null || user.Mobile != null)
                {
                    var sms = new SendSMS();
                    sms.SendLoginSms(mobileNo ?? user.Mobile.Value, code);

                }



                return VoidResult.GetSuccessResult("کد فعالسازی با موفقیت ارسال گردید .");

            }
            catch (Exception e)
            {
                return VoidResult.GetFailResult(e.Message);
            }


        }


        /// <summary>
        /// ثبت نام صنعتگر به سیستم 
        /// </summary>
        [HttpPost]
        [Route("Account/SellerRegister")]
        public LongResult SellerRegister(SellerRegisterDto input)
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
                _logger.LogError(e, e.Message);
                return LongResult.GetFailResult("خطا در سامانه");
            }


        }




    }
}

