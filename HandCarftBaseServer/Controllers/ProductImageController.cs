using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.UIResponse;
using HandCarftBaseServer.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HandCarftBaseServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public ProductImageController(IMapper mapper, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        [Route("ProductImage/GetImageListByProductId")]
        public IActionResult GetImageListByProductId(long productId)
        {
            try
            {

                return Ok(_repository.ProductImage
                    .FindByCondition(c => c.ProductId == productId && c.Ddate == null && c.DaDate == null).ToList());
            }
            catch (Exception e)
            {
                return BadRequest("");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("ProductImage/UploadImage")]
        public IActionResult UploadImage(long productId, int type, string title)
        {
            try
            {

                ProductImage tbl = new ProductImage();

                var a = HttpContext.Request.Form.Files[0];
                var dir = "";
                dir = type == 1 ? "ProductImages" : "ProductVideo";

                var _uploadFileStatus = FileManeger.FileUploader(a, (short)type, dir);

                if (_uploadFileStatus.Status != 200) return BadRequest(_uploadFileStatus.Path);

                tbl.ProductId = productId;
                tbl.Title = title;
                tbl.FileType = type;
                tbl.ImageUrl = _uploadFileStatus.Path;
                tbl.CuserId = ClaimPrincipalFactory.GetUserId(User);
                tbl.Cdate = DateTime.Now.Ticks;

                _repository.ProductImage.Create(tbl);
                _repository.Save();
                return Ok("");


            }
            catch (Exception e)
            {
                return BadRequest("");
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("ProductImage/DeleteProductImage")]
        public IActionResult DeleteProductImage(long productImageId)
        {
            try
            {

                var image = _repository.ProductImage.FindByCondition(c => c.Id == productImageId)
                    .FirstOrDefault();
                var deletedFile = image.ImageUrl;
                if (image == null) return NotFound();
                _repository.ProductImage.Delete(image);
                _repository.Save();
                FileManeger.FileRemover(new List<string> { deletedFile });
                return NoContent();


            }
            catch (Exception e)
            {
                return BadRequest("");
            }
        }

        #region UI_Methods

        /// <summary>
        ///لیست فایل های محصول براساس آیدی محصول
        /// </summary>
        [HttpGet]
        [Route("ProductImage/GetImageListByProductId_UI")]
        public ListResult<ProductImageDto> GetImageListByProductId_UI(long productId)
        {
            try
            {

                var list = _repository.ProductImage
                    .FindByCondition(c => c.ProductId == productId && c.Ddate == null && c.DaDate == null).ToList();

                var result = _mapper.Map<List<ProductImageDto>>(list);

                var finalresult = ListResult<ProductImageDto>.GetSuccessfulResult(result);
                return finalresult;
            }
            catch (Exception e)
            {
                return ListResult<ProductImageDto>.GetFailResult(null);
            }
        }

        #endregion
    }
}
