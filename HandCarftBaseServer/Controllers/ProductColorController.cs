using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ProductColorController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILogger<ProductColorController> _logger;

        public ProductColorController(IMapper mapper, IRepositoryWrapper repository, ILogger<ProductColorController> logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [Route("ProductColor/GetColorListByProductId")]
        public IActionResult GetColorListByProductId(long productId)
        {
            try
            {
                var result = _repository.ProductColor.FindByCondition(c => c.ProductId.Equals(productId))
                    .Include(c => c.Color)
                    .Select(c => new
                    {
                        c.Id,
                        c.Count,
                        c.Price,
                        ColorName = c.Color.Name
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
        [Route("ProductColor/InsertProductColor")]
        public IActionResult InsertProductColor(ProductColor productColor)
        {

            try
            {
                if (_repository.ProductColor.FindByCondition(c =>
                    (c.ProductId == productColor.ProductId) && (c.ColorId == productColor.ColorId)).Any())
                    return BadRequest("رنگ انتخابی برای محصول مورد نظر قبلا ثبت شده است");

                productColor.Cdate = DateTime.Now.Ticks;
                productColor.CuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.ProductColor.Create(productColor);
                _repository.Save();
                return Created("", productColor);

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("ProductColor/GetProductColorById")]
        public IActionResult GetProductColorById(long productColorId)
        {

            try
            {
                var productColor = _repository.ProductColor.FindByCondition(c => c.Id == productColorId)
                    .FirstOrDefault();
                if (productColor==null)
                    return BadRequest("رنگ انتخابی یافت نشد");

                return Ok(productColor);

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("ProductColor/UpdateProductColor")]
        public IActionResult UpdateProductColor(ProductColor productColor)
        {

            try
            {
                var _productColor = _repository.ProductColor.FindByCondition(c =>
                    (c.Id == productColor.Id)).FirstOrDefault();
                if (_productColor == null)
                    return BadRequest("رنگ انتخابی وجود ندارد!");

                _productColor.Price = productColor.Price;
                _productColor.Count = productColor.Count;
                _productColor.Mdate = DateTime.Now.Ticks;
                _productColor.MuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.ProductColor.Update(_productColor);
                _repository.Save();
                return NoContent();

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
