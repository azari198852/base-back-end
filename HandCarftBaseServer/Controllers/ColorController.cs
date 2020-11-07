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
using Microsoft.Extensions.Configuration;

namespace HandCarftBaseServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        public IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public ColorController(IMapper mapper, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;
        }


        [HttpGet]
        [Route("Color/GetColorList")]
        public IActionResult GetColorList()
        {


            try
            {
                var res = _repository.Color.FindByCondition(c => (c.DaDate == null) && (c.Ddate == null)).ToList();
                var result = _mapper.Map<List<ColorDto>>(res);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [HttpGet]
        [Route("Color/GetColorById")]
        public IActionResult GetColorById(long colorId)
        {


            try
            {
                var res = _repository.Color.FindByCondition(c => c.Id == colorId).First();
                var result = _mapper.Map<ColorDto>(res);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("Color/InsertColor")]
        public IActionResult InsertColor(ColorDto color)
        {


            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var _color = _mapper.Map<Color>(color);
                _color.Cdate = DateTime.Now.Ticks;
                _color.CuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.Color.Create(_color);
                _repository.Save();
                return Created("", _color);

            }
            catch (Exception e)
            {

                return BadRequest("Internal Server Error");
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Color/UpdateColor")]
        public IActionResult UpdateColor(ColorDto color)
        {


            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var _color = _repository.Color.FindByCondition(c => c.Id == color.Id).FirstOrDefault();
                if (_color == null) return NotFound();
                _color.Name = color.Name;
                _color.ColorCode = color.ColorCode;
                _color.Rkey = color.Rkey;
                _color.Mdate = DateTime.Now.Ticks;
                _color.MuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.Color.Update(_color);
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
        [Route("Color/DeleteColor")]
        public IActionResult DeleteColor(long colorId)
        {

            try
            {

                var _color = _repository.Color.FindByCondition(c => c.Id == colorId).FirstOrDefault();
                if (_color == null) return NotFound();
                _color.Ddate = DateTime.Now.Ticks;
                _color.DuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.Color.Update(_color);
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
        [Route("Color/DeActiveColor")]
        public IActionResult DeActiveColor(long id)
        {

            try
            {
                var _color = _repository.Color.FindByCondition(c => c.Id == id).FirstOrDefault();
                if (_color == null) return NotFound();
                _color.DaDate = DateTime.Now.Ticks;
                _color.DaUserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.Color.Update(_color);
                _repository.Save();
                return NoContent();

            }
            catch (Exception e)
            {
                return BadRequest("Internal Server Error");
            }
        }


        #region UI_Methods

        /// <summary>
        ///لیست رنگ ها 
        /// </summary>
        [HttpGet]
        [Route("Color/GetColorList_UI")]
        public ListResult<ColorDto> GetColorList_UI()
        {

            try
            {
                var result = _repository.Color.FindAll()
                    .Where(c => c.DaUserId == null && c.DuserId == null)
                    .ToList();
                var s = _mapper.Map<List<ColorDto>>(result);

                var finalresult = ListResult<ColorDto>.GetSuccessfulResult(s);
                return finalresult;

            }
            catch (Exception e)
            {
               return ListResult<ColorDto>.GetFailResult(null);
            }

        }

        #endregion
    }
}
