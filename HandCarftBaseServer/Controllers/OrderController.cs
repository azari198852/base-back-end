using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
        [Route("Order/GetOrderStatusList")]
        public IActionResult GetOrderStatusList()
        {
            try
            {

                var res = _repository.Status
                   .FindByCondition(c => c.CatStatus.Tables.Any(x => x.Name == "CustomerOrder"))
                   .Select(c => new { c.Id, c.Name }).ToList();
                return Ok(res);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        [Route("Order/GetOrderList")]
        public IActionResult GetOrderList()
        {

            try
            {
                var res = _repository.CustomerOrder.FindByCondition(c => c.Ddate == null && c.DaDate == null)
                      .Include(c => c.Customer)
                      .Include(c => c.FinalStatus)
                      .Include(c => c.CustomerOrderProduct)
                      .Include(c => c.CustomerOrderPayment).ThenInclude(c => c.FinalStatus)
                      .Select(c => new
                      {
                          c.Id,
                          OrderDate = DateTimeFunc.TimeTickToShamsi(c.OrderDate.Value),
                          c.OrderNo,
                          OrderType = c.OrderType == 1 ? "خرید" : "سفارش",
                          CustomerName = c.Customer.Name + " " + c.Customer.Fname,
                          c.FinalPrice,
                          Status = c.FinalStatus.Name,
                          PaymentStatus = c.CustomerOrderPayment.OrderByDescending(u => u.Id).Select(x=>x.FinalStatus.Name).FirstOrDefault(),
                          Editable = c.CustomerOrderProduct.All(x => x.FinalStatusId == 23)
                      }).OrderByDescending(c => c.Id).ToList();
                return Ok(res);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("Order/GetOrderFullInfoById")]
        public IActionResult GetOrderFullInfoById(long orderId)
        {

            try
            {
                var orderproduct = _repository.CustomerOrderProduct.FindByCondition(c => c.CustomerOrderId == orderId)
                    .Include(c => c.Product)
                    .Include(c => c.Seller)
                    .Include(c => c.FinalStatus)
                    .Include(c => c.PackingType)
                    .Select(c => new
                    {
                        c.Id,
                        ProductName = c.Product.Name,
                        c.ProductCode,
                        c.ProductPrice,
                        PackingType = c.PackingType.Name,
                        c.OrderCount,
                        Status = c.FinalStatus.Name,
                        Seller = c.Seller.Name + " " + c.Seller.Fname,

                    }).ToList();

                var order = _repository.CustomerOrder.FindByCondition(c => c.Ddate == null && c.DaDate == null && c.Id == orderId)
                    .Include(c => c.Customer)
                    .Include(c => c.FinalStatus)
                    .Include(c => c.CustomerAddress).ThenInclude(c => c.Province)
                    .Include(c => c.CustomerAddress).ThenInclude(c => c.City)
                    .Include(c => c.CustomerOrderPayment).ThenInclude(c => c.FinalStatus)
                    .Select(c => new
                    {

                        OrderDate = DateTimeFunc.TimeTickToShamsi(c.OrderDate.Value),
                        c.OrderNo,
                        OrderType = c.OrderType == 1 ? "خرید" : "سفارش",
                        CustomerName = c.Customer.Name + " " + c.Customer.Fname,
                        c.Customer.Mobile,
                        c.FinalPrice,
                        c.FinalWeight,
                        c.PackingWeight,
                        Status = c.FinalStatus.Name,
                        PaymentStatus = c.CustomerOrderPayment.OrderByDescending(x => x.Id)
                            .Select(x => x.FinalStatus.Name).FirstOrDefault(),
                        Address = c.CustomerAddress.Province.Name + " - " + c.CustomerAddress.City.Name + " - " +
                                  c.CustomerAddress.Address

                    }).FirstOrDefault();
                var result = new { Order = order, Orderproduct = orderproduct };
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Order/UpdateOrderStatus")]
        public IActionResult UpdateOrderStatus(long orderId, int statusId, long? sentDate, string trackingCode, long? deliverDate)
        {

            try
            {
                var order = _repository.CustomerOrder.FindByCondition(c => c.Id == orderId).FirstOrDefault();
                if (order == null) return BadRequest("سفارش یافت نشد.");


                var UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

                if (statusId == 20)
                {
                    order.PostTrackingCode = trackingCode;
                    order.SendDate = UnixEpoch.AddMilliseconds((double)sentDate.Value).Ticks;
                }
                else if (statusId == 21)
                {
                    order.DeliveryDate = UnixEpoch.AddMilliseconds((double)deliverDate.Value).Ticks; ;

                }

                order.FinalStatusId = statusId;
                order.Mdate = DateTime.Now.Ticks;
                order.MuserId = ClaimPrincipalFactory.GetUserId(User);


                var orderstatus = new CustomerOrderStatusLog
                {
                    StatusId = statusId,
                    CustomerOrderId = orderId,
                    Cdate = DateTime.Now.Ticks,
                    CuserId = ClaimPrincipalFactory.GetUserId(User)

                };
                order.CustomerOrderStatusLog.Add(orderstatus);
                _repository.CustomerOrder.Update(order);
                _repository.Save();
                return Ok("");

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }


        #region UI_Methods


        /// <summary>
        ///پیشنمایش سفارش
        /// </summary>
       // [Authorize]
        [HttpPost]
        [Route("Product/CustomerOrderPreview_UI")]
        public SingleResult<OrderPreViewResultDto> CustomerOrderPreview_UI(OrderModel order)
        {
            try
            {

                var orerProductList = new List<CustomerOrderProduct>();



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
                        ProductCode = product.Coding,
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
                var customerOrder = new OrderPreViewResultDto();

                customerOrder.OrderPrice = orerProductList.Sum(c =>
                    (c.ProductPrice + c.PackingPrice - c.ProductOfferPrice) * c.OrderCount);
                customerOrder.OfferPrice =
                    (long?)(customerOrder.OrderPrice * (offer == null ? 0 : offer.Value / 100));
                customerOrder.OfferValue = (int?)offer?.Value;
                customerOrder.FinalWeight = orerProductList.Sum(x => x.FinalWeight);
                customerOrder.OrderWeight = customerOrder.FinalWeight;
                customerOrder.PaymentTypeId = order.PaymentTypeId;
                customerOrder.PostServicePrice = 0;



                customerOrder.ProductList = orerProductList;
                var toCityId = _repository.CustomerAddress.FindByCondition(c => c.Id == order.CustomerAddressId).Include(c => c.City).Select(c => c.City.PostCode).FirstOrDefault();

               
                var postType = _repository.PostType.FindByCondition(c => c.Id == order.PostTypeId).FirstOrDefault();
                var payType = _repository.PaymentType.FindByCondition(c => c.Id == order.PaymentTypeId)
                    .FirstOrDefault();
                if (postType.IsFree.Value)
                {
                    customerOrder.PostServicePrice = 0;
                }
                else
                {
                    var post = new PostServiceProvider();
                    var postpriceparam = new PostGetDeliveryPriceParam
                    {
                        Price = (int)customerOrder.OrderPrice.Value,
                        Weight = (int)customerOrder.FinalWeight.Value,
                        ServiceType = postType?.Rkey ?? 2,// (int)customerOrder.PostTypeId,
                        ToCityId = (int)toCityId,
                        PayType = (int)(payType?.Rkey ?? 88)

                    };
                    var postresult = post.GetDeliveryPrice(postpriceparam).Result;
                    if (postresult.ErrorCode != 0) return SingleResult<OrderPreViewResultDto>.GetFailResult("خطا در دریافت اطلاعات پستی");
                    customerOrder.PostServicePrice = (postresult.PostDeliveryPrice + postresult.VatTax) / 10;

                }


                customerOrder.FinalPrice = customerOrder.OrderPrice + customerOrder.PostServicePrice;

                return SingleResult<OrderPreViewResultDto>.GetSuccessfulResult(customerOrder);


            }
            catch (Exception e)
            {
                return SingleResult<OrderPreViewResultDto>.GetFailResult(e.Message);

            }
        }

        /// <summary>
        ///ثبت سفارش
        /// </summary>
     //   [Authorize]
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
                        PackingTypeId = packingType?.PackinggTypeId,
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

                //customerOrder.TaxPrice = (long?)((customerOrder.OrderPrice - customerOrder.OfferPrice) * 0.09);
                //customerOrder.TaxValue = 9;                
                customerOrder.TaxPrice = 0;
                customerOrder.TaxValue = 9;


                customerOrder.CustomerOrderProduct = orerProductList;
                var toCityId = _repository.CustomerAddress.FindByCondition(c => c.Id == order.CustomerAddressId).Include(c => c.City).Select(c => c.City.PostCode).FirstOrDefault();

              
                var postType = _repository.PostType.FindByCondition(c => c.Id == order.PostTypeId).FirstOrDefault();
                var payType = _repository.PaymentType.FindByCondition(c => c.Id == order.PaymentTypeId)
                    .FirstOrDefault();

                if (postType.IsFree.Value)
                {
                    customerOrder.PostServicePrice = 0;
                }
                else
                {
                    var post = new PostServiceProvider();
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
                    customerOrder.PostServicePrice = (postresult.PostDeliveryPrice + postresult.VatTax) / 10;

                }


                customerOrder.FinalPrice = customerOrder.OrderPrice + customerOrder.TaxPrice + customerOrder.PostServicePrice;
                _repository.CustomerOrder.Create(customerOrder);



                var request = new ZarinPallRequest
                {
                    //  amount = (int)((customerOrder.FinalPrice.Value + customerOrder.PostServicePrice) * 10),
                    amount = (int)((customerOrder.FinalPrice.Value) * 10),
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
                    PaymentPrice = customerOrder.FinalPrice,
                    FinalStatusId = 26

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
        ///پرداخت مبلغ سفارش با آیدی سفارش
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("CustomerOrderPayment/MakePaymentByOrderId")]
        public SingleResult<string> MakePaymentByOrderId(long customerOrderId)
        {

            try
            {
                var userId = ClaimPrincipalFactory.GetUserId(User);
                var customerId = _repository.Customer.FindByCondition(c => c.UserId == userId).Select(c => c.Id).FirstOrDefault();
                var customerOrder = _repository.CustomerOrder.FindByCondition(c => c.Id == customerOrderId && c.CustomerId == customerId).FirstOrDefault();

                if (customerOrder == null)
                {

                    return SingleResult<string>.GetFailResult("سفارش وجود ندارد با سفارش مربوط به کاربر جاری نمی باشد");
                }

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
                    PaymentPrice = customerOrder.FinalPrice,
                    FinalStatusId = 26

                };
                _repository.CustomerOrderPayment.Create(customerOrderPayment);
                _repository.Save();

                return SingleResult<string>.GetSuccessfulResult("https://www.zarinpal.com/pg/StartPay/" + res.authority);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                return SingleResult<string>.GetFailResult(e.Message);


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
                    amount = (int)orderpeymnt.TransactionPrice.Value * 10
                };

                var zarinPal = new ZarinPal();
                var result = zarinPal.VerifyPayment(zarinPalVerifyRequest);
                if (result.code == 100 || result.code == 101)
                {

                    orderpeymnt.FinalStatusId = 27;
                    orderpeymnt.RefNum = result.ref_id.ToString();
                    orderpeymnt.TransactionDate = DateTime.Now.Ticks;
                    orderpeymnt.CardPan = result.card_pan;
                    _repository.CustomerOrderPayment.Update(orderpeymnt);

                    var sendSms = new SendSMS();
                    sendSms.SendSuccessOrderPayment(customer.Mobile.Value, orderpeymnt.OrderNo, customerOrderId.Value);

                    var sendEmail = new SendEmail();
                    var email = customer.Email;
                    sendEmail.SendSuccessOrderPayment(email, orderpeymnt.OrderNo, customerOrderId.Value);
                    _logger.LogInformation($"پرداخت موفق شماره تراکنش {authority}", authority);


                    var productist = _repository.CustomerOrderProduct.FindByCondition(c => c.CustomerOrderId == customerOrderId).Select(c => c.Product).ToList();
                    productist.ForEach(c =>
                    {
                        c.Count = c.Count--;
                        _repository.Product.Update(c);
                    });

                    
                    var sellerList = _repository.CustomerOrderProduct.FindByCondition(c => c.CustomerOrderId == customerOrderId).Select(c => c.Seller.Mobile).ToList();

                    sellerList.ForEach(c =>
                    {
                        if (c != null)
                        {
                            var sendSms = new SendSMS();
                            sendSms.SendOrderSmsForSeller(c.Value);

                        }


                    });


                    _repository.Save();

                    return SingleResult<string>.GetSuccessfulResult("عملیات پرداخت با موفقیت انجام شد.");

                }
                else
                {

                    orderpeymnt.FinalStatusId = 25;
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
                _logger.LogError(e, e.Message);

                return SingleResult<CustomerOrderFullDto>.GetFailResult(e.Message);
            }

        }

        /// <summary>
        ///لیست سفارشات مشتری 
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("CustomerOrder/GetCustomerOrderList_UI")]
        public ListResult<CustomerOrderDto> GetCustomerOrderList_UI(long? finalStatusId)
        {
            try
            {
                var userId = ClaimPrincipalFactory.GetUserId(User);
                var customerId = _repository.Customer.FindByCondition(c => c.UserId == userId).OrderByDescending(c=>c.Cdate).Select(c => c.Id)
                    .FirstOrDefault();
                var res = _repository.CustomerOrder.GetCustomerOrderList(customerId, finalStatusId);
                var result = _mapper.Map<List<CustomerOrderDto>>(res);

                var finalresult = ListResult<CustomerOrderDto>.GetSuccessfulResult(result);
                return finalresult;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                return ListResult<CustomerOrderDto>.GetFailResult(e.Message);
            }

        }

        [Authorize]
        [HttpGet]
        [Route("CustomerOrder/GetCustomerOrderCountGroupByStatus")]
        public ListResult<OrderCountGroupByStatusDto> GetCustomerOrderCountGroupByStatus()
        {
            try
            {
                var userId = ClaimPrincipalFactory.GetUserId(User);
                var customerId = _repository.Customer.FindByCondition(c => c.UserId == userId).Select(c => c.Id)
                    .FirstOrDefault();
                var res = _repository.CustomerOrder
                    .FindByCondition(c => c.CustomerId == customerId && c.Ddate == null && c.DaDate == null)
                    .Include(c => c.FinalStatus).GroupBy(c => c.FinalStatus.Name).Select(g => new OrderCountGroupByStatusDto
                    {
                        Status = g.Key,
                        Count = g.Count()
                    })
                    .ToList();

                var finalresult = ListResult<OrderCountGroupByStatusDto>.GetSuccessfulResult(res);
                return finalresult;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                return ListResult<OrderCountGroupByStatusDto>.GetFailResult(e.Message);
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
                    .FindByCondition(c => c.Ddate == null && c.DaDate == null && c.SellerId == seller.Id && (c.FinalStatusId == 22 || c.FinalStatusId == 23))
                    .Include(c => c.CustomerOrder)
                    .Include(c => c.FinalStatus)
                    .Select(c => new
                    {
                        c.Id,
                        c.ProductName,
                        c.Product.Coding,
                        c.OrderCount,
                        c.CustomerOrder.OrderNo,
                        c.FinalStatusId,
                        Status = c.FinalStatus.Name,
                        Orderdate = DateTimeFunc.TimeTickToShamsi(c.CustomerOrder.OrderDate.Value)
                    }).OrderByDescending(c => c.Id).ToList();

                return Ok(res);

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }

        }


        [Authorize]
        [HttpPut]
        [Route("CustomerOrder/ChangeOrderPruductStatusBySeller")]
        public IActionResult ChangeOrderPruductStatusBySeller(long customerOrderProductId)
        {

            try
            {
                var userid = ClaimPrincipalFactory.GetUserId(User);
                var seller = _repository.Seller.FindByCondition(c => c.UserId == userid || true).FirstOrDefault();
                if (seller == null) return Unauthorized();
                var product = _repository.CustomerOrderProduct.FindByCondition(c => c.Id == customerOrderProductId)
                    .FirstOrDefault();
                if (product == null) return NotFound("کد محصول اشتباه است");
                product.FinalStatusId = 23;
                _repository.CustomerOrderProduct.Update(product);
                _repository.Save();
                return Ok();

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }

        }

        #endregion


    }
}
