using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.UIResponse;
using HandCarftBaseServer.ServiceProvider;
using HandCarftBaseServer.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandCarftBaseServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class PackingTypeController : ControllerBase
    {
        public IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public PackingTypeController(IMapper mapper, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        [Route("PackingType/GetPackingTypeList")]
        public IActionResult GetPackingTypeList()
        {

            try
            {
                var res = _repository.PackingType.FindByCondition(c => (c.DaDate == null) && (c.Ddate == null)).Include(c => c.PackingTypeImage).ToList();
                var result = _mapper.Map<List<PackingTypeDto>>(res);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [HttpGet]
        [Route("PackingType/GetPackingTypeById")]
        public IActionResult GetPackingTypeById(long packingtypeId)
        {

            try
            {
                var res = _repository.PackingType.FindByCondition(c => c.Id == packingtypeId)
                    .Include(c => c.PackingTypeImage).First();
                var result = _mapper.Map<PackingTypeDto>(res);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("PackingType/InsertPackingType")]
        public IActionResult InsertPackingType(PackingTypeDto packingTypeDto)
        {

            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var packingType = _mapper.Map<PackingType>(packingTypeDto);
                packingType.Cdate = DateTime.Now.Ticks;
                packingType.CuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.PackingType.Create(packingType);
                _repository.Save();
                return Created("", packingType);

            }
            catch (Exception e)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [Authorize]
        [HttpPut]
        [Route("PackingType/UpdatePackingType")]
        public IActionResult UpdatePackingType(PackingTypeDto packingTypeDto)
        {

            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var packingType = _repository.PackingType.FindByCondition(c => c.Id == packingTypeDto.Id).FirstOrDefault();
                if (packingType == null) return NotFound();
                packingType.Name = packingTypeDto.Name;
                packingType.Price = packingTypeDto.Price;
                packingType.Weight = packingTypeDto.Weight;
                packingType.Mdate = DateTime.Now.Ticks;
                packingType.MuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.PackingType.Update(packingType);
                _repository.Save();
                return NoContent();

            }
            catch (Exception e)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("PackingType/DeletePackingType")]
        public IActionResult DeletePackingType(long packingtypeId)
        {

            try
            {

                var packingtype = _repository.PackingType.FindByCondition(c => c.Id == packingtypeId).FirstOrDefault();
                if (packingtype == null) return NotFound();
                packingtype.Ddate = DateTime.Now.Ticks;
                packingtype.DuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.PackingType.Update(packingtype);
                _repository.Save();
                return NoContent();


            }
            catch (Exception e)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [HttpGet]
        [Route("PackingType/GetPackingTypeById_UI")]
        public SingleResult<PackingTypeDto> GetPackingTypeById_UI(long packingtypeId)
        {

            try
            {
                var res = _repository.PackingType.FindByCondition(c => c.Id == packingtypeId)
                    .Include(c => c.PackingTypeImage).First();
                var result = _mapper.Map<PackingTypeDto>(res);
                var finalresult = SingleResult<PackingTypeDto>.GetSuccessfulResult(result);
                return finalresult;

            }
            catch (Exception e)
            {
                return SingleResult<PackingTypeDto>.GetFailResult(null);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("PackingType/UploadPackingTypeImage")]
        public IActionResult UploadPackingTypeImage()
        {

            try
            {
                var a = HttpContext.Request.Form.Files[0];

                FileManeger.UploadFileStatus uploadFileStatus = FileManeger.FileUploader(a, 1, "PackingTypeImages");
                var packingTypeImageDto = JsonSerializer.Deserialize<PackingTypeImageDto>(HttpContext.Request.Form["packingTypeImage"]);

                if (uploadFileStatus.Status == 200)
                {


                    var packingTypeImage = _mapper.Map<PackingTypeImage>(packingTypeImageDto);
                    packingTypeImage.Cdate = DateTime.Now.Ticks;
                    packingTypeImage.CuserId = ClaimPrincipalFactory.GetUserId(User);
                    packingTypeImage.ImageFileUrl = uploadFileStatus.Path;
                    _repository.PackingTypeImage.Create(packingTypeImage);
                    _repository.Save();
                    return Created("", packingTypeImage);
                }
                else
                {
                    return BadRequest("");
                }

            }
            catch (Exception e)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("PackingType/DeletePackingTypeImage")]
        public IActionResult DeletePackingTypeImage(long packingTypeImageId)
        {

            try
            {

                var image = _repository.PackingTypeImage.FindByCondition(c => c.Id == packingTypeImageId)
                    .FirstOrDefault();
                var deletedFile = image.ImageFileUrl;
                if (image == null) return NotFound();
                _repository.PackingTypeImage.Delete(image);
                _repository.Save();
                FileManeger.FileRemover(new List<string> { deletedFile });
                return NoContent();


            }
            catch (Exception e)
            {
                return BadRequest("Internal Server Error");
            }
        }

        #region UI_Methods

        /// <summary>
        ///لیست انواع بسته بندی محصول 
        /// </summary>
        [HttpGet]
        [Route("PackingType/GetPackingTypeList_UI")]
        public ListResult<PackingTypeDto> GetPackingTypeList_UI()
        {

            try
            {
                var res = _repository.PackingType.FindByCondition(c => (c.DaDate == null) && (c.Ddate == null)).Include(c => c.PackingTypeImage).ToList();
                var result = _mapper.Map<List<PackingTypeDto>>(res);
                var finalresult = ListResult<PackingTypeDto>.GetSuccessfulResult(result);
                return finalresult;
            }
            catch (Exception e)
            {
                return ListResult<PackingTypeDto>.GetFailResult(null);
            }
        }


        #endregion
    }
}
