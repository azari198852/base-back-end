using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.UIResponse;
using HandCarftBaseServer.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace HandCarftBaseServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CatProductController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILogger<CatProductController> _logger;
        private readonly IConfiguration _configurationonfiguration;


        public CatProductController(IMapper mapper, IRepositoryWrapper repository, ILogger<CatProductController> logger, IConfiguration configurationonfiguration)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
            _configurationonfiguration = configurationonfiguration;
        }

        //[Authorize]
        [HttpPost]
        [Route("CatProduct/InsertCatProduct")]
        public IActionResult InsertCatProduct()
        {
            try
            {

                var catProduct = JsonSerializer.Deserialize<CatProductDto>(HttpContext.Request.Form["catProduct"]);
                var _catProduct = _mapper.Map<CatProduct>(catProduct);

                var icon = HttpContext.Request.Form.Files.GetFile("Icon");
                var cover = HttpContext.Request.Form.Files.GetFile("CoverImage");
                var miniPic = HttpContext.Request.Form.Files.GetFile("miniPic");
                var iconpath = "";
                var coverpath = "";
                var minipath = "";
                if (icon != null)
                {
                    var uploadFileStatus = FileManeger.FileUploader(icon, 1, "CatProductIcon");
                    if (uploadFileStatus.Status == 200)
                    {
                        iconpath = uploadFileStatus.Path;
                    }
                    else
                    {
                        return BadRequest(uploadFileStatus.Path);
                    }
                }

                if (cover != null)
                {
                    var uploadFileStatus1 = FileManeger.FileUploader(cover, 1, "CatProductCover");
                    if (uploadFileStatus1.Status == 200)
                    {
                        coverpath = uploadFileStatus1.Path;
                    }
                    else
                    {
                        return BadRequest(uploadFileStatus1.Path);
                    }

                }

                if (miniPic != null)
                {
                    var uploadFileStatus1 = FileManeger.FileUploader(miniPic, 1, "CatProductMiniPic");
                    if (uploadFileStatus1.Status == 200)
                    {
                        minipath = uploadFileStatus1.Path;
                    }
                    else
                    {
                        return BadRequest(uploadFileStatus1.Path);
                    }

                }

                _catProduct.Icon = iconpath == "" ? null : iconpath;
                _catProduct.Url = coverpath == "" ? null : coverpath;
                _catProduct.MiniPicUrl = minipath == "" ? null : minipath;
                _catProduct.Cdate = DateTime.Now.Ticks;
                _catProduct.CuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.CatProduct.Create(_catProduct);

                _repository.Save();
                return NoContent();


            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest("");
            }


        }

        [Authorize]
        [HttpPut]
        [Route("CatProduct/UpdateCatProduct")]
        public IActionResult UpdateCatProduct()
        {
            try
            {

                var catProduct = JsonSerializer.Deserialize<CatProductDto>(HttpContext.Request.Form["catProduct"]);
                var _catProduct = _repository.CatProduct.FindByCondition(c => c.Id == catProduct.Id).FirstOrDefault();
                if (_catProduct == null) return NotFound();
                _catProduct.Coding = catProduct.Coding;
                _catProduct.Name = catProduct.Name;
                _catProduct.Rkey = catProduct.Rkey;
                _catProduct.MetaTitle = catProduct.MetaTitle;
                _catProduct.MetaDescription = catProduct.MetaDescription;
                _catProduct.KeyWords = catProduct.KeyWords;
                _catProduct.Mdate = DateTime.Now.Ticks;
                _catProduct.MuserId = ClaimPrincipalFactory.GetUserId(User);

                var icon = HttpContext.Request.Form.Files.GetFile("Icon");
                var cover = HttpContext.Request.Form.Files.GetFile("CoverImage");
                var miniPic = HttpContext.Request.Form.Files.GetFile("miniPic");
                List<string> deletedfile = new List<string>();

                if (icon != null)
                {
                    var uploadFileStatus = FileManeger.FileUploader(icon, 1, "CatProductIcon");
                    if (uploadFileStatus.Status == 200)
                    {
                        deletedfile.Add(_catProduct.Icon);
                        _catProduct.Icon = uploadFileStatus.Path;
                    }
                    else
                    {
                        return BadRequest(uploadFileStatus.Path);
                    }
                }

                if (cover != null)
                {
                    var uploadFileStatus1 = FileManeger.FileUploader(cover, 1, "CatProductCover");
                    if (uploadFileStatus1.Status == 200)
                    {
                        deletedfile.Add(_catProduct.Url);
                        _catProduct.Url = uploadFileStatus1.Path;
                    }
                    else
                    {
                        return BadRequest(uploadFileStatus1.Path);
                    }

                }
                if (miniPic != null)
                {
                    var uploadFileStatus1 = FileManeger.FileUploader(miniPic, 1, "CatProductMiniPic");
                    if (uploadFileStatus1.Status == 200)
                    {
                        deletedfile.Add(_catProduct.MiniPicUrl);
                        _catProduct.MiniPicUrl = uploadFileStatus1.Path;
                    }
                    else
                    {
                        return BadRequest(uploadFileStatus1.Path);
                    }

                }

                _repository.CatProduct.Update(_catProduct);
                _repository.Save();
                FileManeger.FileRemover(deletedfile);
                return NoContent();



            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }


        }

        [Authorize]
        [HttpDelete]
        [Route("CatProduct/DeleteCatProduct")]
        public IActionResult DeleteCatProduct(long catProductId)
        {
            try
            {
                var _catProduct = _repository.CatProduct.FindByCondition(c => c.Id == catProductId).FirstOrDefault();
                if (_catProduct == null) return NotFound();
                _catProduct.Ddate = DateTime.Now.Ticks;
                _catProduct.DuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.CatProduct.Update(_catProduct);

                _repository.Save();
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest("");
            }


        }

        [HttpGet]
        [Route("CatProduct/GetCatProductById")]
        public IActionResult GetCatProductById(long catProductId)
        {
            try
            {
                var res = _repository.CatProduct.FindByCondition(c => c.Id == catProductId)
                    .FirstOrDefault();
                if (res == null) return NotFound();
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest("");
            }
        }

        [HttpGet]
        [Route("CatProduct/GetCatProductFullInfoById")]
        public IActionResult GetCatProductFullInfoById(long catProductId)
        {
            try
            {
                var res = _repository.CatProduct.FindByCondition(c => c.Id == catProductId && c.Ddate == null && c.DaDate == null)
                    .Include(c => c.CatProductLanguage).FirstOrDefault();
                if (res == null) return NotFound();
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest("");
            }
        }

        [HttpGet]
        [Route("CatProduct/GetCatProductList")]
        public IActionResult GetCatProductList()
        {

            try
            {
                var list = _repository.CatProduct.FindByCondition(c => c.DaDate == null && c.Ddate == null).ToList();
                var fatherlist = list.Where(c => c.Pid == null).ToList();



                var str = "[";

                foreach (var item in fatherlist)
                {
                    str += "{";
                    str += "'mid':" + item.Id + ",";
                    str += "'text':" + "'" + item.Name + "'";
                    str += GetSecondNode(list, item.Id);
                    str += "},";
                }

                str += "]";


                return Ok(str);
            }
            catch (Exception e)
            {
                return BadRequest("");
            }


        }

        private string GetSecondNode(List<CatProduct> mainlist, long? pid)
        {
            var list = mainlist.Where(c => c.Pid == pid && c.Ddate == null && c.DaDate == null).ToList();
            var str = "";
            if (list.Count > 0)
            {
                str += ",'nodes':";
                str += "[";
                foreach (var item in list)
                {
                    str += "{";
                    str += "'mid':" + item.Id + ",";
                    str += "'text':'" + item.Name + "',";
                    str += GetSecondNode(mainlist, item.Id);
                    str += "},";
                }

                str += "]";

            }


            return str;
        }

        [HttpGet]
        [Route("CatProduct/GetTopCatProductList")]
        public IActionResult GetTopCatProductList()
        {
            try
            {
                var catProduct = _repository.CatProduct.FindByCondition(c => c.Ddate == null && c.DaDate == null)
                    .OrderByDescending(c => c.Product.Count)
                    .Select(c => new { c.Id, c.Name }).ToList().Take(7);

                return Ok(catProduct);
            }
            catch (Exception e)
            {

                return BadRequest("Internal server error");
            }

        }



        [HttpGet]
        [Route("CatProduct/GetCatProductListForCmb")]
        public IActionResult GetCatProductListForCmb()
        {
            var catProduct = _repository.CatProduct.FindByCondition(c => c.DaDate == null && c.Ddate == null)
                .ToList();

            return Ok(catProduct);
        }


        #region UI_Methods

        /// <summary>
        ///لیست دسته بندی محصولات به همراه دسته بندی های زیر مجموعه 
        /// </summary>
        [HttpGet]
        [Route("CatProduct/GetCatProductList_UI")]
        public ListResult<CatProduct> GetCatProductList_UI()
        {
            try
            {
                var catProduct = _repository.CatProduct.FindByCondition(c => c.Ddate == null && c.DaDate == null && c.Pid == null).Include(c => c.InverseP).ToList();
                var finalresult = ListResult<CatProduct>.GetSuccessfulResult(catProduct);
                return finalresult;

            }
            catch (Exception e)
            {
                return ListResult<CatProduct>.GetFailResult(null);
            }

        }


        /// <summary>
        ///لیست دسته بندی های اصلی 
        /// </summary>
        [HttpGet]
        [Route("CatProduct/GetMainCatProductList_UI")]
        public ListResult<CatProductWithCountDto> GetMainCatProductList_UI()
        {
            try
            {
                var catProduct = _repository.CatProduct.FindByCondition(c => c.DaDate == null && c.Ddate == null && c.Pid == null)
                    .Include(c => c.Product)
                    .OrderByDescending(c => c.Product.Count).ToList();

                var result = _mapper.Map<List<CatProductWithCountDto>>(catProduct);

                var finalresult = ListResult<CatProductWithCountDto>.GetSuccessfulResult(result);
                return finalresult;
            }
            catch (Exception e)
            {
                return ListResult<CatProductWithCountDto>.GetFailResult(null);
            }

        }

        /// <summary>
        ///لیست دسته بندی های براساس آیدی پدر 
        /// </summary>
        [HttpGet]
        [Route("CatProduct/GetCatProductListByParentId_UI")]
        public ListResult<CatProductWithCountDto> GetCatProductListByParentId_UI(long catId)
        {
            try
            {


                var catProduct = _repository.CatProduct.FindByCondition(c => c.Pid == catId && c.DaDate == null && c.Ddate == null)
                    .Include(c => c.Product)
                    .OrderByDescending(c => c.Product.Count).ToList();


                var result = _mapper.Map<List<CatProductWithCountDto>>(catProduct);

                var finalresult = ListResult<CatProductWithCountDto>.GetSuccessfulResult(result);
                return finalresult;
            }
            catch (Exception e)
            {
                return ListResult<CatProductWithCountDto>.GetFailResult(e.Message);
            }
        }

        /// <summary>
        ///لیست 5 دسته بندی برتر که بیشترین محصول را دارند 
        /// </summary>
        [HttpGet]
        [Route("CatProduct/GetTopCatProductList_UI")]
        public ListResult<CatProduct> GetTopCatProductList_UI()
        {
            try
            {
                var catProduct = _repository.CatProduct.FindByCondition(c => c.DaDate == null && c.Ddate == null && (c.Id == 2 || c.Id == 44 || c.Id == 45 || c.Id == 73 || c.Id == 75)).OrderByDescending(c => c.Product.Count).Take(5).ToList();

                var finalresult = ListResult<CatProduct>.GetSuccessfulResult(catProduct);
                return finalresult;
            }
            catch (Exception e)
            {
                return ListResult<CatProduct>.GetFailResult(null);
            }



        }

        #endregion


    }
}
