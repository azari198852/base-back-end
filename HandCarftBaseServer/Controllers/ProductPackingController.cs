using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.Models;
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
    public class ProductPackingController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILogger<ProductPackingController> _logger;

        public ProductPackingController(IMapper mapper, IRepositoryWrapper repository, ILogger<ProductPackingController> logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [Route("ProductPacking/GetPackingTypeByProductId")]
        public IActionResult GetPackingTypeByProductId(long productId)
        {
            try
            {
                var result = _repository.ProductPackingType.FindByCondition(c => c.ProductId.Equals(productId))
                    .Include(c => c.PackinggType)
                    .Select(c => new
                    {
                        c.Id,
                        c.Weight,
                        c.Price,
                        PackingName = c.PackinggType.Name
                    }).ToList();
                if (result.Count == 0)
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


        [Authorize]
        [HttpPost]
        [Route("ProductPacking/InsertProductPackingType")]
        public IActionResult InsertProductPackingType(ProductPackingType productPackingType)
        {

            try
            {
                if (_repository.ProductPackingType.FindByCondition(c =>
                    (c.ProductId == productPackingType.ProductId) && (c.PackinggTypeId == productPackingType.PackinggTypeId)).Any())
                    return BadRequest("بسته بندی انتخابی برای محصول مورد نظر قبلا ثبت شده است");

                productPackingType.Cdate = DateTime.Now.Ticks;
                productPackingType.CuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.ProductPackingType.Create(productPackingType);
                _repository.Save();
                return NoContent();

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("ProductPacking/GetProductPackingById")]
        public IActionResult GetProductPackingById(long productPackingTypeId)
        {

            try
            {
                var productPacking = _repository.ProductPackingType.FindByCondition(c => c.Id == productPackingTypeId)
                    .FirstOrDefault();
                if (productPacking == null)
                    return BadRequest("نوع بسته بندی انتخابی یافت نشد");

                return Ok(productPacking);

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("ProductPacking/UpdateProductPacking")]
        public IActionResult UpdateProductPacking(ProductPackingType productPackingType)
        {

            try
            {
                var _productPackingType = _repository.ProductPackingType.FindByCondition(c =>
                    (c.Id == productPackingType.Id)).FirstOrDefault();
                if (_productPackingType == null)
                    return BadRequest("نوع بسته بندی انتخابی وجود ندارد!");

                _productPackingType.Price = productPackingType.Price;
                _productPackingType.Weight = productPackingType.Weight;
                _productPackingType.Mdate = DateTime.Now.Ticks;
                _productPackingType.MuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.ProductPackingType.Update(_productPackingType);
                _repository.Save();
                return NoContent();

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }


        [Authorize]
        [HttpGet]
        [Route("ProductPacking/GetProductPackingImageList")]
        public IActionResult GetProductPackingImageList(long productPackingTypeId)
        {

            try
            {
                var productPackingImage = _repository.ProductPackingTypeImage
                    .FindByCondition(c => c.ProductPackingTypeId == productPackingTypeId)
                    .ToList();


                return Ok(productPackingImage);

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }


        [Authorize]
        [HttpPost]
        [Route("ProductPacking/UploadPackingImage")]
        public IActionResult UploadPackingImage()
        {
            try
            {
                var tbl = JsonSerializer.Deserialize<ProductPackingTypeImage>(HttpContext.Request.Form["ProductPackingTypeImage"]);
                var imageFile = HttpContext.Request.Form.Files.GetFile("PackingTypeImage");

                var _uploadFileStatus = FileManeger.FileUploader(imageFile, 1, "ProductPackingTypeImage");

                if (_uploadFileStatus.Status != 200) return BadRequest(_uploadFileStatus.Path);

                tbl.ImageFileUrl = _uploadFileStatus.Path;
                tbl.CuserId = ClaimPrincipalFactory.GetUserId(User);
                tbl.Cdate = DateTime.Now.Ticks;

                _repository.ProductPackingTypeImage.Create(tbl);
                _repository.Save();
                return Ok("");


            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest("");
            }
        }


        [Authorize]
        [HttpDelete]
        [Route("ProductPacking/DeleteProductPackingImage")]
        public IActionResult DeleteProductPackingImage(long productPackingImageId)
        {
            try
            {

                var image = _repository.ProductPackingTypeImage.FindByCondition(c => c.Id == productPackingImageId)
                    .FirstOrDefault();
                var deletedFile = image.ImageFileUrl;
                if (image == null) return NotFound();
                _repository.ProductPackingTypeImage.Delete(image);
                _repository.Save();
                FileManeger.FileRemover(new List<string> { deletedFile });
                return NoContent();


            }
            catch (Exception e)
            {
                return BadRequest("");
            }
        }

    }


}
