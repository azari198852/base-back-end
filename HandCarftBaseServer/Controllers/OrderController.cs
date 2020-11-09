using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.BusinessModel;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.UIResponse;
using HandCarftBaseServer.ServiceProvider.PostService;
using HandCarftBaseServer.ServiceProvider.ZarinPal;
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
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IMapper mapper, IRepositoryWrapper repository, ILogger<OrderController> logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [Route("Order/GetOrderList")]
        public IActionResult GetOrderList()
        {

            try
            {
                var res = _repository.CustomerOrder.FindByCondition(c => c.Ddate == null && c.DaDate == null)
                      .Include(c => c.Customer)
                      .Include(c => c.FinalStatus).Select(c => new
                      {
                          c.Id,
                          OrderDate = DateTimeFunc.TimeTickToShamsi(c.OrderDate.Value),
                          c.OrderNo,
                          OrderType = c.OrderType == 1 ? "خرید" : "سفارش",
                          CustomerName = c.Customer.Name + " " + c.Customer.Fname,
                          c.FinalPrice,
                          Status = c.FinalStatus.Name

                      }).OrderByDescending(c => c.Id).ToList();
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #region UI_Methods

        /// <summary>
        ///ثبت سفارش
        /// </summary>
        [Authorize]
        [HttpPost]
        [Route("Product/InsertCustomerOrder_UI")]
        public SingleResult<InsertOrderResultDto> GetProductByIdList_UI(OrderModel order)
        {
            try
            {
                var userId = ClaimPrincipalFactory.GetUserId(User);
                var customerId = _repository.Customer.FindByCondition(c => c.UserId == userId).Select(c => c.Id)
                    .FirstOrDefault();
                var today = DateTime.Now.AddDays(-1).Ticks;
                var orerProductList = new List<CustomerOrderProduct>();


                var orderNo = customerId.ToString() + DateTimeFunc.TimeTickToShamsi(DateTime.Now.Ticks).Replace("/", "") +
                        (_repository.CustomerOrder.FindByCondition(c => c.CustomerId == customerId && today > c.Cdate)
                             .Count() + 1).ToString().PadLeft(3, '0');


                order.ProductList.ForEach(c =>
                {
                    var product = _repository.Product.FindByCondition(x => x.Id == c.ProductId).First();
                    var ofer = _repository.Offer.FindByCondition(x => x.Id == c.OfferId).FirstOrDefault();
                    var packingType = _repository.ProductPackingType.FindByCondition(x => x.Id == c.PackingTypeId)
                        .FirstOrDefault();
                    var statusId = _repository.Status.GetSatusId("CustomerOrderProduct", 2);
                    var orderproduct = new CustomerOrderProduct
                    {
                        OrderCount = c.Count,
                        ProductId = c.ProductId,
                        Cdate = DateTime.Now.Ticks,
                        CuserId = userId,
                        OrderType = 1,
                        ProductCode = product.Coding,
                        ProductIncreasePrice = null,
                        ProductName = product.Name,
                        ProductPrice = product.Price,
                        ProductOfferId = c.OfferId,
                        ProductOfferCode = ofer?.OfferCode,
                        ProductOfferPrice = (long?)(ofer != null ? (ofer.Value / 100 * product.Price) : 0),
                        ProductOfferValue = ofer?.Value,
                        PackingTypeId = c.PackingTypeId,
                        PackingWeight = packingType == null ? 0 : packingType.Weight,
                        PackingPrice = packingType == null ? 0 : packingType.Price,
                        SellerId = product.SellerId,
                        Weight = c.Count * product.Weight,
                        FinalWeight = (c.Count * product.Weight) + (c.Count * (packingType == null ? 0 : packingType.Weight)),
                        FinalStatusId = statusId

                    };
                    orerProductList.Add(orderproduct);

                });

                var offer = _repository.Offer.FindByCondition(c => c.Id == order.OfferId).FirstOrDefault();
                var paking = _repository.PackingType.FindByCondition(c => c.Id == order.PaymentTypeId).FirstOrDefault();
                var customerOrder = new CustomerOrder
                {
                    Cdate = DateTime.Now.Ticks,
                    CuserId = userId,
                    CustomerAddressId = order.CustomerAddressId,
                    CustomerDescription = order.CustomerDescription,
                    CustomerId = customerId,
                    FinalStatusId = _repository.Status.GetSatusId("CustomerOrder", 1),
                    OfferId = order.OfferId,
                    OrderPrice = orerProductList.Sum(x =>
                        ((x.PackingPrice + x.ProductPrice - x.ProductOfferPrice) * x.OrderCount))
                };

                customerOrder.OrderPrice = orerProductList.Sum(c =>
                    (c.ProductPrice + c.PackingPrice - c.ProductOfferPrice) * c.OrderCount);
                customerOrder.OfferPrice =
                    (long?)(customerOrder.OrderPrice * (offer == null ? 0 : offer.Value / 100));
                customerOrder.OfferValue = (int?)offer?.Value;
                customerOrder.OrderDate = DateTime.Now.Ticks;
                customerOrder.FinalWeight = orerProductList.Sum(x => x.FinalWeight);
                customerOrder.OrderNo = Convert.ToInt64(orderNo);
                customerOrder.OrderProduceTime = 0;
                customerOrder.OrderType = 1;
                customerOrder.OrderWeight = customerOrder.FinalWeight;
                customerOrder.PackingPrice = 0;
                customerOrder.PackingWeight = 0;
                customerOrder.PaymentTypeId = order.PaymentTypeId;
                customerOrder.PostServicePrice = 0;
                customerOrder.PostTypeId = order.PostTypeId;

                customerOrder.TaxPrice = (long?)((customerOrder.OrderPrice - customerOrder.OfferPrice) * 0.09);
                customerOrder.TaxValue = 9;
                customerOrder.FinalPrice = customerOrder.OrderPrice + customerOrder.TaxPrice;

                customerOrder.CustomerOrderProduct = orerProductList;
                var toCityId = _repository.CustomerAddress.FindByCondition(c => c.Id == order.CustomerAddressId).Include(c => c.City).Select(c => c.City.PostCode).FirstOrDefault();

                var post = new PostServiceProvider();
                var postType = _repository.PostType.FindByCondition(c => c.Id == order.PostTypeId).FirstOrDefault();
                var payType = _repository.PaymentType.FindByCondition(c => c.Id == order.PaymentTypeId)
                    .FirstOrDefault();
                var postpriceparam = new PostGetDeliveryPriceParam
                {
                    Price = (int)customerOrder.OrderPrice.Value,
                    Weight = (int)customerOrder.FinalWeight.Value,
                    ServiceType = postType?.Rkey ?? 2,// (int)customerOrder.PostTypeId,
                    ToCityId = (int)toCityId,
                    PayType = (int)(payType?.Rkey ?? 88)

                };
                var postresult = post.GetDeliveryPrice(postpriceparam).Result;
                if (postresult.ErrorCode != 0) return SingleResult<InsertOrderResultDto>.GetFailResult("خطا در دریافت اطلاعات پستی");
                customerOrder.PostServicePrice = postresult.PostDeliveryPrice + postresult.VatTax;
                _repository.CustomerOrder.Create(customerOrder);



                var request = new ZarinPallRequest
                {
                    amount = (int)((customerOrder.FinalPrice.Value + customerOrder.PostServicePrice) * 10),
                    description = "order NO: " + customerOrder.OrderNo
                };
                var zarinPal = new ZarinPal();
                var res = zarinPal.Request(request);

                var customerOrderPayment = new CustomerOrderPayment
                {
                    OrderNo = customerOrder.OrderNo.ToString(),
                    TraceNo = res.authority,
                    TransactionPrice = customerOrder.FinalPrice,
                    TransactionDate = DateTime.Now.Ticks,
                    Cdate = DateTime.Now.Ticks,
                    CuserId = userId,
                    PaymentPrice = customerOrder.FinalPrice
                };
                customerOrder.CustomerOrderPayment.Add(customerOrderPayment);
                _repository.Save();

                var result = new InsertOrderResultDto
                {
                    OrderNo = customerOrder.OrderNo,
                    CustomerOrderId = customerOrder.Id,
                    BankUrl = "https://www.zarinpal.com/pg/StartPay/" + res.authority,
                    RedirectToBank = true,
                    PostPrice = customerOrder.PostServicePrice
                };

                return SingleResult<InsertOrderResultDto>.GetSuccessfulResult(result);


            }
            catch (Exception e)
            {
                return SingleResult<InsertOrderResultDto>.GetFailResult(e.Message);

            }
        }

        /// <summary>
        ///استعلام پرداخت از بانک
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("CustomerOrderPayment/VerifyPayment_UI")]
        public SingleResult<string> VerifyPayment(string authority, string status)
        {
            try
            {
                var orderpeymnt = _repository.CustomerOrderPayment.FindByCondition(c => c.TraceNo == authority)
                    .FirstOrDefault();
                if (orderpeymnt == null)
                {
                    return SingleResult<string>.GetFailResult("اطلاعاتی برای پارامترهای ارسالی یافت نشد.");
                }
                var customerOrderId = orderpeymnt.CustomerOrderId;
                var customer = _repository.CustomerOrder.FindByCondition(c => c.Id == customerOrderId)
                    .Include(c => c.Customer).Select(c => c.Customer).First();

                var zarinPalVerifyRequest = new ZarinPalVerifyRequest
                {
                    authority = authority,
                    amount = (int)orderpeymnt.TransactionPrice.Value
                };

                var zarinPal = new ZarinPal();
                var result = zarinPal.VerifyPayment(zarinPalVerifyRequest);
                if (result.code == 100 || result.code == 101)
                {

                    orderpeymnt.FinalStatusId = 100;
                    orderpeymnt.RefNum = result.ref_id.ToString();
                    orderpeymnt.TransactionDate = DateTime.Now.Ticks;
                    orderpeymnt.CardPan = result.card_pan;
                    _repository.CustomerOrderPayment.Update(orderpeymnt);
                    _repository.Save();
                    var sendSms = new SendSMS();

                    sendSms.SendSuccessOrderPayment("0" + customer.Mobile.ToString(), orderpeymnt.OrderNo, customerOrderId.Value);
                    var sendEmail = new SendEmail();
                    var email = customer.Email;
                    sendEmail.SendSuccessOrderPayment(email, orderpeymnt.OrderNo, customerOrderId.Value);
                    _logger.LogInformation($"پرداخت موفق شماره تراکنش {authority}", authority);
                    return SingleResult<string>.GetSuccessfulResult("عملیات پرداخت با موفقیت انجام شد.");

                }
                else
                {

                    orderpeymnt.FinalStatusId = result.code;
                    orderpeymnt.TransactionDate = DateTime.Now.Ticks;
                    _repository.CustomerOrderPayment.Update(orderpeymnt);
                    _repository.Save();
                    _logger.LogInformation($"پرداخت ناموفق شماره تراکنش {authority}", authority);
                    return SingleResult<string>.GetFailResult("پرداخت نا موفق");

                }


            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return SingleResult<string>.GetFailResult("خطا در سامانه");
            }
        }

        /// <summary>
        ///اطلاعات سفارش مشتری براساس آیدی سفارش
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("CustomerOrder/GetCustomerOrder_byId_UI")]
        public SingleResult<CustomerOrderFullDto> GetCustomerOrder_byId_UI(long orderId)
        {

            try
            {
                var res = _repository.CustomerOrder.GetCustomerOrderFullInfoById(orderId);
                var result = _mapper.Map<CustomerOrderFullDto>(res);

                var finalresult = SingleResult<CustomerOrderFullDto>.GetSuccessfulResult(result);
                return finalresult;
            }
            catch (Exception e)
            {
                return SingleResult<CustomerOrderFullDto>.GetFailResult(e.Message);
            }

        }

        /// <summary>
        ///لیست سفارشات مشتری 
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("CustomerOrder/GetCustomerOrderList_UI")]
        public ListResult<CustomerOrderDto> GetCustomerOrderList_UI()
        {
            try
            {
                var userId = ClaimPrincipalFactory.GetUserId(User);
                var customerId = _repository.Customer.FindByCondition(c => c.UserId == userId).Select(c => c.Id)
                    .FirstOrDefault();
                var res = _repository.CustomerOrder.GetCustomerOrderList(customerId);
                var result = _mapper.Map<List<CustomerOrderDto>>(res);

                var finalresult = ListResult<CustomerOrderDto>.GetSuccessfulResult(result);
                return finalresult;
            }
            catch (Exception e)
            {
                return ListResult<CustomerOrderDto>.GetFailResult(e.Message);
            }

        }

        #endregion


        #region Seller_Methods

        [Authorize]
        [HttpGet]
        [Route("CustomerOrder/GetOrderListForSeller")]
        public IActionResult GetOrderListForSeller()
        {

            try
            {
                var userid = ClaimPrincipalFactory.GetUserId(User);
                var seller = _repository.Seller.FindByCondition(c => c.UserId == userid).FirstOrDefault();
                if (seller == null) return Unauthorized();
                var res = _repository.CustomerOrderProduct
                    .FindByCondition(c => c.Ddate == null && c.DaDate == null && c.SellerId == seller.Id)
                    .Include(c => c.CustomerOrder)
                    .Select(c => new
                    {
                        c.Id,
                        c.ProductName,
                        c.Product.Coding,
                        c.OrderCount,
                        c.CustomerOrder.OrderNo,
                        Orderdate = DateTimeFunc.TimeTickToShamsi(c.CustomerOrder.OrderDate.Value)
                    });

                return Ok(res);

            }
            catch (Exception e)
            {
                return BadRequest("");
            }

        }

        #endregion


    }
}
