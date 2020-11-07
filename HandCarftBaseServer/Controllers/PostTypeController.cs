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
    public class PostTypeController : ControllerBase
    {
        public IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ISyncDataBaseService _sync;

        public PostTypeController(IMapper mapper, IRepositoryWrapper repository, ISyncDataBaseService sync)
        {
            _mapper = mapper;
            _repository = repository;
            _sync = sync;
        }

        [HttpGet]
        [Route("PostType/GetPostTypeList")]
        public IActionResult GetPostTypeList()
        {

            try
            {
                var res = _repository.PostType.FindByCondition(c => (c.DaDate == null) && (c.Ddate == null)).ToList();
                var result = _mapper.Map<List<PostTypeDto>>(res);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [HttpGet]
        [Route("PostType/GetPostTypeById")]
        public IActionResult GetPostTypeById(long postTypeId)
        {


            try
            {
                var res = _repository.PostType.FindByCondition(c => c.Id == postTypeId).First();
                var result = _mapper.Map<PostTypeDto>(res);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("PostType/InsertPostType")]
        public IActionResult InsertPostType(PostTypeDto postTypeDto)
        {


            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var postType = _mapper.Map<PostType>(postTypeDto);
                postType.Cdate = DateTime.Now.Ticks;
                postType.CuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.PostType.Create(postType);
                _repository.Save();
                return Created("", postType);

            }
            catch (Exception e)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [Authorize]
        [HttpPut]
        [Route("PostType/UpdatePostType")]
        public IActionResult UpdatePostType(PostTypeDto postTypeDto)
        {


            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var postType = _repository.PostType.FindByCondition(c => c.Id == postTypeDto.Id).FirstOrDefault();
                if (postType == null) return NotFound();
                postType.Title = postTypeDto.Title;
                postType.Duration = postTypeDto.Duration;
                postType.Description = postTypeDto.Description;
                postType.ApiUrl = postTypeDto.ApiUrl;
                postType.Icon = postTypeDto.Icon;
                postType.IsFree = postTypeDto.IsFree;
                postType.Price = postTypeDto.Price;
                postType.Rkey = postTypeDto.Rkey;
                postType.Mdate = DateTime.Now.Ticks;
                postType.MuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.PostType.Update(postType);
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
        [Route("PostType/DeletePostType")]
        public IActionResult DeletePostType(long postTypeId)
        {

            try
            {

                var postType = _repository.PostType.FindByCondition(c => c.Id == postTypeId).FirstOrDefault();
                if (postType == null) return NotFound();
                postType.Ddate = DateTime.Now.Ticks;
                postType.DuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.PostType.Update(postType);
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
        [Route("PostType/DeActivePostType")]
        public IActionResult DeActivePostType(long postTypeId)
        {

            try
            {
                var postType = _repository.PostType.FindByCondition(c => c.Id == postTypeId).FirstOrDefault();
                if (postType == null) return NotFound();
                postType.DaDate = DateTime.Now.Ticks;
                postType.DaUserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.PostType.Update(postType);
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
        ///لیست نوع ارسال محصول 
        /// </summary>
        [HttpGet]
        [Route("PostType/GetPostTypeList_UI")]
        public ListResult<PostTypeDto> GetPostTypeList_UI()
        {

            try
            {
                var res = _repository.PostType.FindByCondition(c => (c.DaDate == null) && (c.Ddate == null)).ToList();
                var result = _mapper.Map<List<PostTypeDto>>(res);

                var finalresult = ListResult<PostTypeDto>.GetSuccessfulResult(result);
                return finalresult;
            }
            catch (Exception e)
            {
                return ListResult<PostTypeDto>.GetFailResult(null);
            }
        }

        /// <summary>
        ///نوع ارسال محصول براساس آیدی 
        /// </summary>
        [HttpGet]
        [Route("PostType/GetPostTypeById_UI")]
        public SingleResult<PostTypeDto> GetPostTypeById_UI(long postTypeId)
        {


            try
            {
                var res = _repository.PostType.FindByCondition(c => c.Id == postTypeId).First();
                var result = _mapper.Map<PostTypeDto>(res);

                var finalresult = SingleResult<PostTypeDto>.GetSuccessfulResult(result);
                return finalresult;
            }
            catch (Exception e)
            {
                return SingleResult<PostTypeDto>.GetFailResult(null);
            }
        }



        #endregion
    }
}
