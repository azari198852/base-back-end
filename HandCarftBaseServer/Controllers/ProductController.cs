using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
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
        private readonly ILogger<ProductController> _logger;

        public ProductController(IMapper mapper, IRepositoryWrapper repository, ILogger<ProductController> logger)
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
                    .OrderByDescending(c=>c.Id).ToList());
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
                _logger.LogError(e, e.Message);
                return BadRequest("Internal server error");
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




        #region UI_Methods

        /// <summary>
        ///لیست  محصولات با صفحه بندی و فیلتر
        /// </summary>
        /// <param name="sortMethod">
        /// 1: مرتب سازی بر اساس جدیدترین(پیشفرض )
        /// 2: مرتب سازی بر اساس امتیاز
        /// 3: مرتب سازی بر اساس پرفروشترین
        /// 4: مرتب سازی بر اساس ارزانترین
        /// 5: مرتب سازی بر اساس گرانترین
        /// 6: دارای نشان ملی
        /// </param>
        /// <param name="catProductId">
        /// آیدی دسته بندی محصول
        /// </param>
        ///  /// <param name="productName">
        /// نام محصول 
        /// </param>
        /// <param name="minPrice">
        /// حداقل قیمت 
        /// </param>
        ///  <param name="maxPrice">
        /// حداکثر قیمت 
        /// </param>
        /// <param name="pageSize">
        /// سایز صفحه بندی 
        /// </param>
        /// <param name="pageNumber">
        /// شماره صفحه  
        /// </param>
        [HttpGet]
        [Route("Product/GetProductList_Paging_Filtering_UI")]
        public ListResult<ProductDto> GetProductList_Paging_Filtering_UI(long? catProductId, string productName, long? minPrice, long? maxPrice, short sortMethod, int pageSize, int pageNumber)
        {
            try
            {
                List<Product> res = new List<Product>();
                int totalcount = 0;
                switch (sortMethod)
                {

                    case 2:
                        res = _repository.Product.GetProductListFullInfo()
                            .Where(c => (productName == null || c.Name.Contains(productName)) &&
                                        (c.CatProductId == catProductId || catProductId == null) &&
                                        (minPrice < c.Price || minPrice == null) &&
                                        (c.Price < maxPrice || maxPrice == null))
                            .OrderByDescending(c => c.ProductCustomerRate.Average(x => x.Rate)).ToList();
                        totalcount = res.Count;
                        res = res.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                        break;

                    case 3:
                        res = _repository.Product.GetProductListFullInfo()
                            .Where(c => (productName == null || c.Name.Contains(productName)) &&
                                        (c.CatProductId == catProductId || catProductId == null) &&
                                        (minPrice < c.Price || minPrice == null) &&
                                        (c.Price < maxPrice || maxPrice == null))
                            .OrderByDescending(c => c.CustomerOrderProduct.Count()).ToList();
                        totalcount = res.Count;
                        res = res.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                        break;

                    case 4:
                        res = _repository.Product.GetProductListFullInfo()
                            .Where(c => (productName == null || c.Name.Contains(productName)) &&
                                        (c.CatProductId == catProductId || catProductId == null) &&
                                        (minPrice < c.Price || minPrice == null) &&
                                        (c.Price < maxPrice || maxPrice == null))
                            .OrderBy(c => c.Price).ToList();
                        totalcount = res.Count;
                        res = res.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                        break;

                    case 5:
                        res = _repository.Product.GetProductListFullInfo()
                            .Where(c => (productName == null || c.Name.Contains(productName)) &&
                                        (c.CatProductId == catProductId || catProductId == null) &&
                                        (minPrice < c.Price || minPrice == null) &&
                                        (c.Price < maxPrice || maxPrice == null))
                            .OrderByDescending(c => c.Price).ToList();
                        totalcount = res.Count;
                        res = res.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                        break;

                    case 6:
                        res = _repository.Product.GetProductListFullInfo()
                            .Where(c => (productName == null || c.Name.Contains(productName)) &&
                                        (c.CatProductId == catProductId || catProductId == null) &&
                                        (minPrice < c.Price || minPrice == null) &&
                                        (c.Price < maxPrice || maxPrice == null) && c.MelliFlag == true)
                            .OrderByDescending(c => c.Cdate).ToList();
                        totalcount = res.Count;
                        res = res.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                        break;
                    default:
                        res = _repository.Product.GetProductListFullInfo()
                            .Where(c => (productName == null || c.Name.Contains(productName)) &&
                                        (c.CatProductId == catProductId || catProductId == null) &&
                                        (minPrice < c.Price || minPrice == null) &&
                                        (c.Price < maxPrice || maxPrice == null))
                            .OrderByDescending(c => c.Cdate).ToList();
                        totalcount = res.Count;
                        res = res.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                        break;

                }


                var result = _mapper.Map<List<ProductDto>>(res);

                var finalresult = ListResult<ProductDto>.GetSuccessfulResult(result, totalcount);
                return finalresult;

            }
            catch (Exception e)
            {
                return ListResult<ProductDto>.GetFailResult(null);

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
                return finalresult;

            }
            catch (Exception e)
            {
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
                    .Include(c => c.ProductPackingTypeImage).ToList();

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
                                                               (c.FinalStatusId == 7) || (c.FinalStatusId == 9) || (c.FinalStatusId == 10) || (c.FinalStatusId == 11))
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
                        c.Price,
                        c.Weight,
                        c.ProduceDuration,
                        c.ProducePrice,


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
                    return NotFound();

                }
                product.Mdate = DateTime.Now.Ticks;
                product.MuserId = ClaimPrincipalFactory.GetUserId(User);
                product.Price = sellerProductUpdateModel.Price;
                product.ProducePrice = sellerProductUpdateModel.ProducePrice;
                product.ProduceDuration = sellerProductUpdateModel.ProduceDuration;
                product.FinalStatusId = 8;
                _repository.Product.Update(product);
                _repository.Save();

                return NoContent();


            }
            catch (Exception e)
            {

                _logger.LogError(e, e.Message);
                return BadRequest("Internal server error");
            }
        }

        #endregion


    }
}
