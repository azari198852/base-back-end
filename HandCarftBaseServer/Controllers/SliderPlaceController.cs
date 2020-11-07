using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using HandCarftBaseServer.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HandCarftBaseServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class SliderPlaceController : ControllerBase
    {
        public IMapper _mapper;
        private readonly IRepositoryWrapper _repository;


        public SliderPlaceController(IMapper mapper, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;

        }

        [HttpGet]
        [Route("SliderPlace/GetSliderPlaceList")]
        public IActionResult GetSliderPlaceList()
        {


            try
            {
                var res = _repository.SliderPlace.FindByCondition(c => (c.DaDate == null) && (c.Ddate == null)).ToList();
                var result = _mapper.Map<List<SliderPlaceDto>>(res);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [HttpGet]
        [Route("SliderPlace/GetSliderPlaceById")]
        public IActionResult GetSliderPlaceById(long sliderPlaceId)
        {


            try
            {
                var res = _repository.SliderPlace.FindByCondition(c => c.Id == sliderPlaceId).FirstOrDefault();
                if (res == null) return NotFound("");
                var result = _mapper.Map<SliderPlaceDto>(res);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("SliderPlace/InsertSliderPlace")]
        public IActionResult InsertSliderPlace(SliderPlaceDto sliderPlaceDto)
        {


            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var sliderPlace = _mapper.Map<SliderPlace>(sliderPlaceDto);
                sliderPlace.Cdate = DateTime.Now.Ticks;
                sliderPlace.CuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.SliderPlace.Create(sliderPlace);
                _repository.Save();

                return Created("", sliderPlace);

            }
            catch (Exception e)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [Authorize]
        [HttpPut]
        [Route("SliderPlace/UpdateSliderPlace")]
        public IActionResult UpdateSliderPlace(SliderPlaceDto sliderPlaceDto)
        {


            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var sliderPlace = _repository.SliderPlace.FindByCondition(c => c.Id == sliderPlaceDto.Id).FirstOrDefault();
                if (sliderPlace == null) return NotFound();
                sliderPlace.Name = sliderPlaceDto.Name;
                sliderPlace.Rkey = sliderPlaceDto.Rkey;
                sliderPlace.Mdate = DateTime.Now.Ticks;
                sliderPlace.MuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.SliderPlace.Update(sliderPlace);
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
        [Route("SliderPlace/DeleteSliderPlace")]
        public IActionResult DeleteSliderPlace(long sliderPlaceId)
        {

            try
            {

                var sliderPlace = _repository.SliderPlace.FindByCondition(c => c.Id == sliderPlaceId).FirstOrDefault();
                if (sliderPlace == null) return NotFound();
                sliderPlace.Ddate = DateTime.Now.Ticks;
                sliderPlace.DuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.SliderPlace.Update(sliderPlace);
                _repository.Save();

                return NoContent();


            }
            catch (Exception e)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [Authorize]
        [HttpPut]
        [Route("SliderPlace/DeActiveSliderPlace")]
        public IActionResult DeActiveSliderPlace(long sliderPlaceId)
        {

            try
            {
                var sliderPlace = _repository.SliderPlace.FindByCondition(c => c.Id == sliderPlaceId).FirstOrDefault();
                if (sliderPlace == null) return NotFound();
                sliderPlace.DaDate = DateTime.Now.Ticks;
                sliderPlace.DaUserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.SliderPlace.Update(sliderPlace);
                _repository.Save();

                return NoContent();

            }
            catch (Exception e)
            {
                return BadRequest("Internal Server Error");
            }
        }

      


    }
}
