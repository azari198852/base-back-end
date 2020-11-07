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
using HandCarftBaseServer.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandCarftBaseServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class SliderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;


        public SliderController(IMapper mapper, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;

        }

        [Authorize]
        [HttpPost]
        [Route("Slider/InsertSlider")]
        public IActionResult InsertSlider()
        {
            var sliderdto = JsonSerializer.Deserialize<SliderDto>(HttpContext.Request.Form["Slider"]);
            var _slider = _mapper.Map<Slider>(sliderdto);
            var imageUrl = HttpContext.Request.Form.Files[0];

            var uploadFileStatus = FileManeger.FileUploader(imageUrl, 1, "SliderImages");

            if (uploadFileStatus.Status != 200) return BadRequest("Internal server error");

            _slider.ImageUrl = uploadFileStatus.Path;
            _slider.CuserId = ClaimPrincipalFactory.GetUserId(User);
            _slider.Cdate = DateTime.Now.Ticks;
            _repository.Slider.Create(_slider);



            try
            {
                _repository.Save();
                return Created("", _slider);
            }
            catch (Exception e)
            {

                FileManeger.FileRemover(new List<string> { uploadFileStatus.Path });
                return BadRequest(e.Message.ToString());
            }


        }

        [Authorize]
        [HttpPut]
        [Route("Slider/UpdateSlider")]
        public IActionResult UpdateSlider()
        {

            var sliderdto = JsonSerializer.Deserialize<SliderDto>(HttpContext.Request.Form["Slider"]);
            var _slider = _mapper.Map<Slider>(sliderdto);
            var slider = _repository.Slider.FindByCondition(c => c.Id.Equals(_slider.Id)).FirstOrDefault();
            if (slider == null)
            {

                return NotFound();
            }

            if (HttpContext.Request.Form.Files.Count > 0)
            {
                var imageUrl = HttpContext.Request.Form.Files[0];
                var deletedFile = slider.ImageUrl;
                var uploadFileStatus = FileManeger.FileUploader(imageUrl, 1, "SliderImages");
                if (uploadFileStatus.Status != 200) return BadRequest("Internal server error");
                slider.ImageHurl = _slider.ImageHurl;
                slider.ImageUrl = uploadFileStatus.Path;
                slider.LinkUrl = _slider.LinkUrl;
                slider.MuserId = ClaimPrincipalFactory.GetUserId(User);
                slider.Mdate = DateTime.Now.Ticks;
                slider.Rorder = _slider.Rorder;
                slider.SliderPlaceId = _slider.SliderPlaceId;
                slider.Title = _slider.Title;
                _repository.Slider.Update(slider);



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


            slider.ImageHurl = _slider.ImageHurl;
            slider.LinkUrl = _slider.LinkUrl;
            slider.MuserId = ClaimPrincipalFactory.GetUserId(User);
            slider.Mdate = DateTime.Now.Ticks;
            slider.Rorder = _slider.Rorder;
            slider.SliderPlaceId = _slider.SliderPlaceId;
            slider.Title = _slider.Title;
            _repository.Slider.Update(slider);
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

        [Authorize]
        [HttpDelete]
        [Route("Slider/DeleteSlider")]
        public IActionResult DeleteSlider(long sliderId)
        {

            try
            {
                var slider = _repository.Slider.FindByCondition(c => c.Id.Equals(sliderId)).FirstOrDefault();
                if (slider == null)
                {

                    return NotFound();

                }
                slider.Ddate = DateTime.Now.Ticks;
                slider.DuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.Slider.Update(slider);
                _repository.Save();
                return NoContent();


            }
            catch (Exception e)
            {


                return BadRequest("Internal server error");
            }
        }


        [HttpGet]
        [Route("Slider/GetSliderById")]
        public IActionResult GetSliderById(long sliderId)
        {
            try
            {
                var res = _repository.Slider.FindByCondition(c => c.Id == sliderId)
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
        [Route("Slider/GetSliderList")]
        public IActionResult GetSliderList(long sliderPlaceId)
        {
            try
            {
                var res = _repository.Slider.FindByCondition(c => (c.Ddate == null && c.DaDate == null && (c.SliderPlaceId == sliderPlaceId || sliderPlaceId == -1))).Include(c => c.SliderPlace)
                    .ToList();
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        #region UI_Methods

        /// <summary>
        ///لیست اسلایدرها براساس آیدی محل اسلایدر
        /// </summary>
        [HttpGet]
        [Route("Slider/GetSliderByPlaceCode_UI")]
        public ListResult<SliderDto> GetSliderByPlaceCode_UI(long sliderPlaceCode)
        {

            try
            {
                var slider = _repository.Slider.FindByCondition(s => s.SliderPlace.Rkey.Equals(sliderPlaceCode) && s.SliderPlace.Ddate == null && s.DaDate == null && s.Ddate == null)
                    .OrderByDescending(c => c.Rorder).ToList();
                if (slider.Count.Equals(0))
                {

                    return ListResult<SliderDto>.GetFailResult("اطلاعاتی برای کد ارسال شده یافت نشد.");

                }

                var a = _mapper.Map<List<SliderDto>>(slider);
                var finalresult = ListResult<SliderDto>.GetSuccessfulResult(a);
                return finalresult;

            }
            catch (Exception e)
            {

                return ListResult<SliderDto>.GetFailResult(null);
            }

        }

        #endregion
    }
}
