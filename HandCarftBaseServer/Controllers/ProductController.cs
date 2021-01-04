using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.BusinessModel;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.Params;
using Entities.UIResponse;
using HandCarftBaseServer.Tools;
using Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace HandCarftBaseServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILogHandler _logger;

        public ProductController(IMapper mapper, IRepositoryWrapper repository, ILogHandler logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [Route("Product/GetProductList")]
        public IActionResult GetProductList()
        {
            try
            {
                return Ok(_repository.Product.FindByCondition(c => c.Ddate == null && c.DaDate == null)
                    .Include(c => c.CatProduct)
                    .Include(c => c.Seller)
                    .Select(c => new
                    {
                        c.Id,
                        c.Name,
                        Seller = c.Seller.Name + " " + c.Seller.Fname,
                        c.CatProductId,
                        CatProductName = c.CatProduct.Name,
                        c.Rkey,
                        c.Coding,
                        c.Price,
                        c.Weight


                    })
                    .OrderByDescending(c => c.Id).ToList());
            }
            catch (Exception e)
            {
                return BadRequest("");
            }
        }

        [HttpGet]
        [Route("Product/GetProductParamList")]
        public IActionResult GetProductParamList(long productId)
        {
            try
            {

                var res = _repository.ProductCatProductParameters.GetProductParamList(productId);
                var result = _mapper.Map<List<ProductParamDto>>(res);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest("");
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Product/UpdateProductParam")]
        public IActionResult UpdateProductParam(List<ProductParamDto> paramList)
        {
            try
            {
                var updatelist = _mapper.Map<List<ProductCatProductParameters>>(paramList);
                updatelist.ForEach(c =>
                {
                    c.Mdate = DateTime.Now.Ticks;
                    c.MuserId = ClaimPrincipalFactory.GetUserId(User);
                    _repository.ProductCatProductParameters.Update(c);
                });

                _repository.Save();
                return NoContent();


            }
            catch (Exception e)
            {
                return BadRequest("");
            }
        }

        [HttpGet]
        [Route("Product/GetProductStatusList")]
        public IActionResult GetProductStatusList()
        {
            try
            {
                var result = _repository.Status.FindByCondition(c =>
                    c.CatStatus.Tables.Any(x => x.Name == "Product") && c.Ddate == null && c.DaDate == null).ToList();

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest("");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("Product/InsertProduct")]
        public IActionResult InsertProduct()
        {
            var coverpath = "";
            var downloadpath = "";
            try
            {

                var product = JsonSerializer.Deserialize<Product>(HttpContext.Request.Form["Product"]);
                var releventList = JsonSerializer.Deserialize<List<long>>(HttpContext.Request.Form["releventList"]);
                var coverImageUrl = HttpContext.Request.Form.Files.GetFile("CoverImage");
                var downloadLink = HttpContext.Request.Form.Files.GetFile("DownloadLink");



                if (coverImageUrl != null)
                {
                    var uploadFileStatus = FileManeger.FileUploader(coverImageUrl, 1, "ProductImages");
                    if (uploadFileStatus.Status == 200)
                    {
                        coverpath = uploadFileStatus.Path;
                    }
                    else
                    {
                        return BadRequest(uploadFileStatus.Path);
                    }
                }
                if (downloadLink != null && product.VirtualProduct.Value)
                {
                    var uploadFileStatus = FileManeger.FileUploader(downloadLink, 3, "ProductFiles");
                    if (uploadFileStatus.Status == 200)
                    {
                        downloadpath = uploadFileStatus.Path;
                    }
                    else
                    {
                        return BadRequest(uploadFileStatus.Path);
                    }
                }


                product.CoverImageUrl = coverpath;
                product.DownloadLink = downloadpath;
                product.CuserId = ClaimPrincipalFactory.GetUserId(User);
                product.Cdate = DateTime.Now.Ticks;

                var counter = (_repository.Product
                                  .FindByCondition(c => c.Coding.ToString().Substring(0, 9) == product.Coding.ToString())
                                  .Count() + 1).ToString();
                counter = counter.PadLeft(3, '0');

                product.Coding = Convert.ToInt64(string.Concat(product.Coding.ToString(), counter));

                product.LastSeenDate = DateTime.Now.Ticks;
                product.SeenCount = 0;


                var parameters = _repository.CatProductParameters
                    .FindByCondition(c => c.CatProductId == product.CatProductId).ToList();

                parameters.ForEach(c =>
                {
                    var paramss = new ProductCatProductParameters
                    {
                        CatProductParametersId = c.Id,
                        CuserId = ClaimPrincipalFactory.GetUserId(User),
                        Cdate = DateTime.Now.Ticks
                    };
                    product.ProductCatProductParameters.Add(paramss);

                });
                releventList.ForEach(c =>
                {
                    var pro = new RelatedProduct
                    {
                        CuserId = ClaimPrincipalFactory.GetUserId(User),
                        Cdate = DateTime.Now.Ticks,
                        DestinProductId = c
                    };
                    product.RelatedProductOriginProduct.Add(pro);

                });

                _repository.Product.Create(product);


                _repository.Save();

                return NoContent();



            }
            catch (Exception e)
            {
                FileManeger.FileRemover(new List<string> { coverpath, downloadpath });
                _logger.LogError(e, MethodBase.GetCurrentMethod());
                return BadRequest(e.Message);
            }

        }

        [Authorize]
        [HttpPut]
        [Route("Product/EditProduct")]
        public IActionResult EditProduct(long productId)
        {


            Product _product = JsonSerializer.Deserialize<Product>(HttpContext.Request.Form["Product"]);
            Seller seller = new Seller();
            var userid = ClaimPrincipalFactory.GetUserId(User);

            var product = _repository.Product.FindByCondition(c => c.Id.Equals(productId)).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }

            if (_product.SellerId == null || _product.SellerId == 0)
            {
                seller = _repository.Seller.FindByCondition(c => c.UserId == userid).FirstOrDefault();
            }
            else
            {
                seller = _repository.Seller.FindByCondition(c => c.Id == _product.SellerId).FirstOrDefault();
            }

            if (product.SellerId != seller.Id || product.CatProductId != _product.CatProductId)
            {
                var catProduct = _repository.CatProduct.FindByCondition(c => c.Id == _product.CatProductId)
                    .FirstOrDefault();
                var counter = (_repository.Product
                                   .FindByCondition(c => c.SellerId == seller.Id && c.CatProductId == catProduct.Id)
                                   .Count() + 1).ToString();
                counter = counter.PadLeft(3, '0');
                product.Coding = long.Parse(seller.SellerCode.ToString() + catProduct.Coding.ToString() + counter);

            }

            product.Name = _product.Name;
            product.EnName = _product.EnName;
            product.CatProductId = _product.CatProductId;
            product.Coding = _product.Coding;
            product.Price = _product.Price;
            product.FirstCount = _product.FirstCount;
            product.ProductMeterId = _product.ProductMeterId;
            product.Description = _product.Description;
            product.SellerId = seller.Id;
            product.MuserId = userid;
            product.Mdate = DateTime.Now.Ticks;

            if (HttpContext.Request.Form.Files.Count > 0)
            {
                var coverImageUrl = HttpContext.Request.Form.Files[0];
                var deletedFile = product.CoverImageUrl;
                FileManeger.UploadFileStatus uploadFileStatus = FileManeger.FileUploader(coverImageUrl, 1, "ProductImages");
                if (uploadFileStatus.Status == 200)
                {

                    product.CoverImageUrl = uploadFileStatus.Path;
                    _repository.Product.Update(product);
                    try
                    {
                        _repository.Save();
                        FileManeger.FileRemover(new List<string> { deletedFile });

                        return NoContent();
                    }
                    catch (Exception e)
                    {


                        FileManeger.FileRemover(new List<string> { uploadFileStatus.Path });
                        return BadRequest("Internal server error");
                    }

                }
                else
                {

                    return BadRequest("Internal server error");
                }
            }
            else
            {

                _repository.Product.Update(product);
                try
                {
                    _repository.Save();

                    return NoContent();
                }
                catch (Exception e)
                {


                    return BadRequest("Internal server error");
                }


            }


        }

        [Authorize]
        [HttpDelete]
        [Route("Product/DeleteProduct")]
        public IActionResult DeleteProduct(long productId)
        {

            try
            {
                var product = _repository.Product.FindByCondition(c => c.Id.Equals(productId)).FirstOrDefault();
                if (product == null)
                {
                    return NotFound();

                }
                product.Ddate = DateTime.Now.Ticks;
                product.DuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.Product.Update(product);
                _repository.Save();

                return NoContent();


            }
            catch (Exception e)
            {


                return BadRequest("Internal server error");
            }
        }

        [HttpPut]
        [Route("Product/DeActiveProduct")]
        public IActionResult DeActiveProduct(long productId)
        {

            try
            {
                var product = _repository.Product.FindByCondition(c => c.Id.Equals(productId)).FirstOrDefault();
                if (product == null)
                {
                    return NotFound();

                }
                product.DaDate = DateTime.Now.Ticks;
                product.DaUserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.Product.Update(product);
                _repository.Save();

                return NoContent();


            }
            catch (Exception e)
            {


                return BadRequest("Internal server error");
            }
        }

        [HttpGet]
        [Route("Product/GetProductById")]
        public IActionResult GetProductById(long productId)
        {
            try
            {
                var result = _repository.Product.FindByCondition(c => c.Id.Equals(productId)).FirstOrDefault();
                if (result.Equals(null))
                {

                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception e)
            {


                return BadRequest("Internal server error");
            }

        }

        /// <summary>
        /// لیست محصولات براساس کد فروشنده و نام محصول  
        /// </summary>
        [HttpGet]
        [Route("Product/GetProductListFilterBySeller_Name")]
        public IActionResult GetProductById(long? sellerId, string productName)
        {
            try
            {
                var result = _repository.Product.FindByCondition(c => c.Ddate == null && c.DaDate == null && (c.SellerId == sellerId || sellerId == null) && (c.Name.Contains(productName) || string.IsNullOrWhiteSpace(productName))).Select(c => new { c.Id, c.Name, }).ToList();
                _logger.LogData(MethodBase.GetCurrentMethod(), result, null, sellerId, productName);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, MethodBase.GetCurrentMethod(), sellerId, productName);
                return BadRequest(e.Message);
            }

        }

        /// <summary>
        /// لیست محصولات براساس فیلترها  
        /// </summary>
        [HttpGet]
        [Route("Product/GetProductListByFilter")]
        public IActionResult GetProductListByFilter(GetProductListByFilterParams filter)
        {
            try
            {
                var result = _repository.Product.FindByCondition(c => c.Ddate == null && c.DaDate == null &&
                                                                      (filter.CatProductIds.Contains(c.CatProductId.Value) || filter.CatProductIds.Count == 0) &&
                                                                      (filter.SellerIds.Contains(c.SellerId.Value) || filter.SellerIds.Count == 0) &&
                                                                      (filter.ProductIds.Contains(c.Id) || filter.ProductIds.Count == 0) &&
                                                                      (filter.FromPrice <= c.Price || filter.FromPrice == null) &&
                                                                      (filter.ToPrice >= c.Price || filter.ToPrice == null))
                                                                      .Include(c => c.CatProduct).Include(c => c.Seller)
                                                                      .Select(c => new { c.Id, c.Name, c.Coding, c.Count, CatProduct = c.CatProduct.Name, Seller = c.Seller.Name + " " + c.Seller.Fname })
                                                                      .OrderByDescending(c => c.Id).AsNoTracking().ToList();
                _logger.LogData(MethodBase.GetCurrentMethod(), result, null, filter);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, MethodBase.GetCurrentMethod(), filter);
                return BadRequest(e.Message);
            }

        }


        #region UI_Methods

        /// <summary>
        ///لیست  محصولات با صفحه بندی و فیلتر
        /// </summary>
        /// <param name="filter.sortMethod">
        /// 1: مرتب سازی بر اساس جدیدترین(پیشفرض )
        /// 2: مرتب سازی بر اساس امتیاز
        /// 3: مرتب سازی بر اساس پرفروشترین
        /// 4: مرتب سازی بر اساس ارزانترین
        /// 5: مرتب سازی بر اساس گرانترین
        /// 6: دارای نشان ملی
        /// 7: دارای مهر اصالت یونسکو
        /// </param>
        /// <param name="filter.catProductId">
        /// آیدی دسته بندی محصول
        /// </param>
        ///  /// <param name="filter.productName">
        /// نام محصول 
        /// </param>
        /// <param name="filter.minPrice">
        /// حداقل قیمت 
        /// </param>
        ///  <param name="filter.maxPrice">
        /// حداکثر قیمت 
        /// </param>
        /// <param name="filter.pageSize">
        /// سایز صفحه بندی 
        /// </param>
        /// <param name="filter.pageNumber">
        /// شماره صفحه  
        /// </param>
        [HttpPost]
        [Route("Product/GetProductList_Paging_Filtering_UI")]
        public SingleResult<ProductListDto> GetProductList_Paging_Filtering_UI(ProductListParam filter)
        {
            try
            {
                List<Product> res = new List<Product>();
                int totalcount = 0;
                long? MinPrice = 0;
                long? MaxPrice = 0;
                switch (filter.SortMethod)
                {

                    case 2:
                        res = _repository.Product.GetProductListFullInfo()
                            .Where(c => (filter.ProductName == null || c.Name.Contains(filter.ProductName)) &&
                                        (c.CatProductId == filter.CatProductId || filter.CatProductId == null) &&
                                        (filter.MinPrice <= c.Price || filter.MinPrice == null) &&
                                        (c.Price <= filter.MaxPrice || filter.MaxPrice == null) &&
                                        (filter.SellerIdList.Contains(c.SellerId.Value) || filter.SellerIdList.Count == 0))
                            .OrderByDescending(c => c.ProductCustomerRate.Average(x => x.Rate)).ToList();
                        MinPrice = res.Min(c => c.Price);
                        MaxPrice = res.Max(c => c.Price);
                        totalcount = res.Count;
                        res = res.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
                        break;

                    case 3:
                        res = _repository.Product.GetProductListFullInfo()
                            .Where(c => (filter.ProductName == null || c.Name.Contains(filter.ProductName)) &&
                                        (c.CatProductId == filter.CatProductId || filter.CatProductId == null) &&
                                        (filter.MinPrice <= c.Price || filter.MinPrice == null) &&
                                        (c.Price <= filter.MaxPrice || filter.MaxPrice == null) &&
                                        (filter.SellerIdList.Contains(c.SellerId.Value) || filter.SellerIdList.Count == 0))
                            .OrderByDescending(c => c.CustomerOrderProduct.Count()).ToList();
                        MinPrice = res.Min(c => c.Price);
                        MaxPrice = res.Max(c => c.Price);
                        totalcount = res.Count;
                        res = res.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
                        break;

                    case 4:
                        res = _repository.Product.GetProductListFullInfo()
                            .Where(c => (filter.ProductName == null || c.Name.Contains(filter.ProductName)) &&
                                        (c.CatProductId == filter.CatProductId || filter.CatProductId == null) &&
                                        (filter.MinPrice <= c.Price || filter.MinPrice == null) &&
                                        (c.Price <= filter.MaxPrice || filter.MaxPrice == null) &&
                                        (filter.SellerIdList.Contains(c.SellerId.Value) || filter.SellerIdList.Count == 0))
                            .OrderBy(c => c.Price).ToList();
                        MinPrice = res.Min(c => c.Price);
                        MaxPrice = res.Max(c => c.Price);
                        totalcount = res.Count;
                        res = res.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
                        break;

                    case 5:
                        res = _repository.Product.GetProductListFullInfo()
                            .Where(c => (filter.ProductName == null || c.Name.Contains(filter.ProductName)) &&
                                        (c.CatProductId == filter.CatProductId || filter.CatProductId == null) &&
                                        (filter.MinPrice <= c.Price || filter.MinPrice == null) &&
                                        (c.Price <= filter.MaxPrice || filter.MaxPrice == null) &&
                                        (filter.SellerIdList.Contains(c.SellerId.Value) || filter.SellerIdList.Count == 0))
                            .OrderByDescending(c => c.Price).ToList();
                        MinPrice = res.Min(c => c.Price);
                        MaxPrice = res.Max(c => c.Price);
                        totalcount = res.Count;
                        res = res.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
                        break;

                    case 6:
                        res = _repository.Product.GetProductListFullInfo()
                            .Where(c => c.MelliFlag == true &&
                                        (filter.ProductName == null || c.Name.Contains(filter.ProductName)) &&
                                        (c.CatProductId == filter.CatProductId || filter.CatProductId == null) &&
                                        (filter.MinPrice <= c.Price || filter.MinPrice == null) &&
                                        (c.Price <= filter.MaxPrice || filter.MaxPrice == null) &&
                                        (filter.SellerIdList.Contains(c.SellerId.Value) || filter.SellerIdList.Count == 0))
                            .OrderByDescending(c => c.Cdate).ToList();
                        MinPrice = res.Min(c => c.Price);
                        MaxPrice = res.Max(c => c.Price);
                        totalcount = res.Count;
                        res = res.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
                        break;
                    case 7:
                        res = _repository.Product.GetProductListFullInfo()
                            .Where(c => c.UnescoFlag == true &&
                                        (filter.ProductName == null || c.Name.Contains(filter.ProductName)) &&
                                        (c.CatProductId == filter.CatProductId || filter.CatProductId == null) &&
                                        (filter.MinPrice <= c.Price || filter.MinPrice == null) &&
                                        (c.Price <= filter.MaxPrice || filter.MaxPrice == null) &&
                                        (filter.SellerIdList.Contains(c.SellerId.Value) || filter.SellerIdList.Count == 0))
                            .OrderByDescending(c => c.Cdate).ToList();
                        MinPrice = res.Min(c => c.Price);
                        MaxPrice = res.Max(c => c.Price);
                        totalcount = res.Count;
                        res = res.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
                        break;
                    default:
                        res = _repository.Product.GetProductListFullInfo()
                            .Where(c => (filter.ProductName == null || c.Name.Contains(filter.ProductName)) &&
                                        (c.CatProductId == filter.CatProductId || filter.CatProductId == null) &&
                                        (filter.MinPrice <= c.Price || filter.MinPrice == null) &&
                                        (c.Price <= filter.MaxPrice || filter.MaxPrice == null) &&
                                        (filter.SellerIdList.Contains(c.SellerId.Value) || filter.SellerIdList.Count == 0))
                             .OrderByDescending(c => c.Cdate).ToList();
                        MinPrice = res.Min(c => c.Price);
                        MaxPrice = res.Max(c => c.Price);
                        totalcount = res.Count;
                        res = res.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
                        break;

                }

                ProductListDto result = new ProductListDto
                {
                    MaxPrice = MaxPrice,
                    MinPrice = MinPrice,
                    TotalCount = totalcount,
                    ProductList = _mapper.Map<List<ProductDto>>(res)
                };



                var finalresult = SingleResult<ProductListDto>.GetSuccessfulResult(result);
                return finalresult;

            }
            catch (Exception e)
            {
                return SingleResult<ProductListDto>.GetFailResult(null);

            }
        }

        /// <summary>
        ///لیست 10 محصول آخر که مهر یونسکو دارند
        /// </summary>
        [HttpGet]
        [Route("Product/GetProductList_HaveUnescoCode_UI")]
        public ListResult<ProductDto> GetProductList_HaveUnescoCode_UI()
        {
            try
            {


                var res = _repository.Product.GetProductListFullInfo().Where(c => c.UnescoFlag == true)
                    .OrderByDescending(c => c.Cdate).Take(10).ToList();
                var result = _mapper.Map<List<ProductDto>>(res);

                var finalresult = ListResult<ProductDto>.GetSuccessfulResult(result);
                _logger.LogData(MethodBase.GetCurrentMethod(), finalresult, null);
                return finalresult;

            }
            catch (Exception e)
            {
                _logger.LogError(e, MethodBase.GetCurrentMethod());
                return ListResult<ProductDto>.GetFailResult(null);

            }
        }

        /// <summary>
        ///لیست 10 محصول آخر که نشان ملی دارند
        /// </summary>
        [HttpGet]
        [Route("Product/GetProductList_HaveMelliCode_UI")]
        public ListResult<ProductDto> GetProductList_HaveMelliCode_UI()
        {
            try
            {


                var res = _repository.Product.GetProductListFullInfo().Where(c => c.MelliFlag == true)
                    .OrderByDescending(c => c.Cdate).Take(10).ToList();
                var result = _mapper.Map<List<ProductDto>>(res);

                var finalresult = ListResult<ProductDto>.GetSuccessfulResult(result);
                return finalresult;

            }
            catch (Exception e)
            {
                return ListResult<ProductDto>.GetFailResult(null);

            }
        }

        /// <summary>
        ///لیست 10 محصول آخر ثبت شده
        /// </summary>
        [HttpGet]
        [Route("Product/GetProductList_latest_UI")]
        public ListResult<ProductDto> GetProductList_latest_UI()
        {
            try
            {

                var res = _repository.Product.GetProductListFullInfo().OrderByDescending(c => c.Cdate).Take(10).ToList();
                var result = _mapper.Map<List<ProductDto>>(res);

                var finalresult = ListResult<ProductDto>.GetSuccessfulResult(result);
                return finalresult;

            }
            catch (Exception e)
            {
                return ListResult<ProductDto>.GetFailResult(null);

            }
        }

        /// <summary>
        ///لیست رنگبندی محصول براساس آیدی محصول
        /// </summary>
        [HttpGet]
        [Route("Product/GetProductColorList_UI")]
        public ListResult<ProductColorDto> GetProductColorList_UI(long productId)
        {
            try
            {

                var res = _repository.ProductColor.FindByCondition(c => c.Ddate == null && c.DaDate == null && c.ProductId == productId).Include(c => c.Color).ToList();

                var result = _mapper.Map<List<ProductColorDto>>(res);
                var finalresult = ListResult<ProductColorDto>.GetSuccessfulResult(result);
                return finalresult;

            }
            catch (Exception e)
            {
                return ListResult<ProductColorDto>.GetFailResult(null);

            }
        }

        /// <summary>
        ///لیست نوع بسته بندی های محصول براساس آیدی محصول
        /// </summary>
        [HttpGet]
        [Route("Product/GetProductPackingTypeList_UI")]
        public ListResult<ProductPackingTypeDto> GetProductPackingTypeList_UI(long productId)
        {
            try
            {

                var res = _repository.ProductPackingType.FindByCondition(c => c.Ddate == null && c.DaDate == null && c.ProductId == productId)
                    .Include(c => c.ProductPackingTypeImage).Include(c => c.PackinggType).ToList();

                var result = _mapper.Map<List<ProductPackingTypeDto>>(res);
                var finalresult = ListResult<ProductPackingTypeDto>.GetSuccessfulResult(result);
                return finalresult;

            }
            catch (Exception e)
            {
                return ListResult<ProductPackingTypeDto>.GetFailResult(null);

            }
        }

        /// <summary>
        ///لیست محصولات مرتبط با محصول براساس آیدی محصول
        /// </summary>
        [HttpGet]
        [Route("Product/GetRelatedProductList_UI")]
        public ListResult<Product> GetRelatedProductList_UI(long productId)
        {
            try
            {

                var res = _repository.RelatedProduct.FindByCondition(c => c.Ddate == null && c.DaDate == null && c.OriginProductId == productId)
                    .Include(c => c.DestinProduct).Select(c => c.DestinProduct).ToList();


                var finalresult = ListResult<Product>.GetSuccessfulResult(res);
                return finalresult;

            }
            catch (Exception e)
            {
                return ListResult<Product>.GetFailResult(null);

            }
        }

        /// <summary>
        ///لیست پارامترهای محصول براساس آیدی محصول
        /// </summary>
        [HttpGet]
        [Route("Product/GetProductParamList_UI")]
        public ListResult<ProductParamDto> GetProductParamList_UI(long productId)
        {
            try
            {

                var res = _repository.ProductCatProductParameters.GetProductParamList(productId);
                var result = _mapper.Map<List<ProductParamDto>>(res);

                var finalresult = ListResult<ProductParamDto>.GetSuccessfulResult(result);
                return finalresult;

            }
            catch (Exception e)
            {
                return ListResult<ProductParamDto>.GetFailResult(null);

            }
        }


        /// <summary>
        ///لیست محصولات  براساس آیدی دسته بندی
        /// </summary>
        [HttpGet]
        [Route("Product/GetProductListByCatId_UI")]
        public ListResult<ProductDto> GetProductListByCatId_UI(long catProductId)
        {
            try
            {

                var res = _repository.Product.GetProductListFullInfo().Where(c => c.CatProductId == catProductId).ToList();

                var result = _mapper.Map<List<ProductDto>>(res);

                var finalresult = ListResult<ProductDto>.GetSuccessfulResult(result);
                return finalresult;

            }
            catch (Exception e)
            {
                return ListResult<ProductDto>.GetFailResult(null);

            }
        }

        /// <summary>
        ///لیست 10 محصولی که اخیرا بازدید شده اند
        /// </summary>
        [HttpGet]
        [Route("Product/GetProductList_LastSeen_UI")]
        public ListResult<ProductDto> GetProductList_LastSeen_UI()
        {
            try
            {

                var res = _repository.Product.GetProductListFullInfo().OrderByDescending(c => c.SeenCount).Take(10)
                    .ToList();

                var result = _mapper.Map<List<ProductDto>>(res);
                var finalresult = ListResult<ProductDto>.GetSuccessfulResult(result);
                return finalresult;

            }
            catch (Exception e)
            {
                return ListResult<ProductDto>.GetFailResult(null);

            }
        }

        /// <summary>
        ///لیست نظرات درباره محصول براساس آیدی محصول
        /// </summary>
        [HttpGet]
        [Route("Product/GetProductCommentList_UI")]
        public ListResult<ProductCustomerRateDto> GetProductCommentList_UI(long productId)
        {
            try
            {

                var res = _repository.ProductCustomerRate.GetProductCommentFullInfo(productId).OrderByDescending(c => c.Mdate)
                    .ToList();

                var result = _mapper.Map<List<ProductCustomerRateDto>>(res);
                var finalresult = ListResult<ProductCustomerRateDto>.GetSuccessfulResult(result);
                return finalresult;

            }
            catch (Exception e)
            {
                return ListResult<ProductCustomerRateDto>.GetFailResult(null);

            }
        }


        /// <summary>
        ///مشخصات محصول براساس آیدی محصول
        /// </summary>
        [HttpGet]
        [Route("Product/GetProductById_UI")]
        public SingleResult<ProductDto> GetProductById_UI(long productId)
        {
            try
            {

                var res = _repository.Product.GetProductListFullInfo().FirstOrDefault(c => c.Id == productId);
                if (res == null) return SingleResult<ProductDto>.GetFailResult("محصولی با آیدی فرستاده شده یتفت نشد!");
                var result = _mapper.Map<ProductDto>(res);

                var finalresult = SingleResult<ProductDto>.GetSuccessfulResult(result);

                var product = _repository.Product.FindByCondition(c => c.Id == productId).FirstOrDefault();
                product.LastSeenDate = DateTime.Now.Ticks;
                product.SeenCount += 1;
                _repository.Product.Update(product);
                _repository.Save();

                return finalresult;

            }
            catch (Exception e)
            {
                return SingleResult<ProductDto>.GetFailResult(null);

            }
        }

        /// <summary>
        ///مشخصات محصول براساس لیست آیدی محصول
        /// </summary>
        [HttpPost]
        [Route("Product/GetProductByIdList_UI")]
        public ListResult<ProductDto> GetProductByIdList_UI(List<long> productIdList)
        {
            try
            {

                var res = _repository.Product.GetProductListFullInfo().Where(c => productIdList.Contains(c.Id)).ToList();
                if (res.Count == 0) return ListResult<ProductDto>.GetFailResult("محصولی با آیدی فرستاده شده یافت نشد!");
                var result = _mapper.Map<List<ProductDto>>(res);

                var finalresult = ListResult<ProductDto>.GetSuccessfulResult(result);
                return finalresult;

            }
            catch (Exception e)
            {
                return ListResult<ProductDto>.GetFailResult(null);

            }
        }

        /// <summary>
        ///جستجو کلی محصولات
        /// </summary>
        [HttpGet]
        [Route("Product/ProducatGeneralSearch_UI")]
        public ListResult<ProductGeneralSearchResultDto> ProducatGeneralSearch_UI(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name) || name.Length < 3)
                    return ListResult<ProductGeneralSearchResultDto>.GetSuccessfulResult(null);

                var res = _repository.Product.FindByCondition(c => c.Ddate == null && c.DaDate == null && c.Name.Contains(name)).Include(c => c.CatProduct).ToList();
                var result = _mapper.Map<List<ProductGeneralSearchResultDto>>(res);
                var finalresult = ListResult<ProductGeneralSearchResultDto>.GetSuccessfulResult(result, result.Count);
                return finalresult;

            }
            catch (Exception e)
            {
                return ListResult<ProductGeneralSearchResultDto>.GetFailResult(null);

            }
        }

        #endregion

        #region Seller_Methods

        [Authorize]
        [HttpGet]
        [Route("Product/GetSellerProductList")]
        public IActionResult GetSellerProductList()
        {
            try
            {
                var userid = ClaimPrincipalFactory.GetUserId(User);
                var seller = _repository.Seller.FindByCondition(c => c.UserId == userid).FirstOrDefault();
                if (seller == null) return Unauthorized();
                return Ok(_repository.Product.FindByCondition(c => c.Ddate == null && c.DaDate == null && c.SellerId == seller.Id &&
                                                               (c.FinalStatusId == 7) || (c.FinalStatusId == 8) || (c.FinalStatusId == 9) || (c.FinalStatusId == 10) || (c.FinalStatusId == 11))
                    .Include(c => c.CatProduct)
                    .Include(c => c.FinalStatus)
                    .Select(c => new
                    {
                        c.Id,
                        c.Name,
                        CatProductName = c.CatProduct.Name,
                        Status = c.FinalStatus.Name,
                        c.FinalStatusId,
                        c.Coding,
                        c.Count,
                        c.Price,
                        c.Weight,
                        c.ProduceDuration,
                        c.ProducePrice,
                        c.CoverImageUrl

                    })
                    .ToList());
            }
            catch (Exception e)
            {
                return BadRequest("");
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Product/UpdateProductBySeller")]
        public IActionResult UpdateProductBySeller(SellerProductUpdateModel sellerProductUpdateModel)
        {

            try
            {
                var product = _repository.Product.FindByCondition(c => c.Id.Equals(sellerProductUpdateModel.ProductId)).FirstOrDefault();
                if (product == null)
                {
                    throw new BusinessException(XError.GetDataErrors.NotFound());

                }
                product.Mdate = DateTime.Now.Ticks;
                product.MuserId = ClaimPrincipalFactory.GetUserId(User);
                product.Price = sellerProductUpdateModel.Price;
                product.CanHaveOrder = sellerProductUpdateModel.CanHaveOrder;
                product.ProducePrice = sellerProductUpdateModel.ProducePrice;
                product.ProduceDuration = sellerProductUpdateModel.ProduceDuration;
                product.Count = sellerProductUpdateModel.Count;
                product.FinalStatusId = 8;
                _repository.Product.Update(product);
                _repository.Save();

                _logger.LogData(MethodBase.GetCurrentMethod(), General.Results_.SuccessMessage(), null, sellerProductUpdateModel);
                return Ok(General.Results_.SuccessMessage());


            }
            catch (Exception e)
            {

                _logger.LogError(e, MethodBase.GetCurrentMethod(), sellerProductUpdateModel);
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("Product/GetProductByIdForSeller")]
        public IActionResult GetProductByIdForSeller(long productId)
        {

            try
            {
                var product = _repository.Product.FindByCondition(c => c.Id.Equals(productId))
                    .Include(c => c.CatProduct)
                    .Select(c => new
                    {
                        c.Id,
                        c.Name,
                        CatProductName = c.CatProduct.Name,
                        c.FinalStatusId,
                        c.Coding,
                        c.Count,
                        c.Price,
                        c.Weight,
                        c.ProduceDuration,
                        c.ProducePrice,


                    }).FirstOrDefault();
                if (product == null)
                {
                    throw new BusinessException(XError.GetDataErrors.NotFound());

                }

                var imageList = _repository.ProductImage
                    .FindByCondition(c => c.ProductId == productId && c.DaDate == null && c.Ddate == null).ToList();
                var finalres = new { Product = product, ImageList = imageList };
                _logger.LogData(MethodBase.GetCurrentMethod(), finalres, null, productId);
                return Ok(finalres);


            }
            catch (Exception e)
            {

                _logger.LogError(e, MethodBase.GetCurrentMethod(), productId);
                return BadRequest(e.Message);
            }
        }

        #endregion


    }
}
