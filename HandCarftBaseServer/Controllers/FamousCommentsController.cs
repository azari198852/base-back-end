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

namespace HandCarftBaseServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class FamousCommentsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public FamousCommentsController(IMapper mapper, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [Authorize]
        [HttpPost]
        [Route("FamousComments/InsertFamousComments")]
        public IActionResult InsertFamousComments()
        {
            var famousCommentsDto = JsonSerializer.Deserialize<FamousCommentsDto>(HttpContext.Request.Form["FamousComments"]);
            var famousComments = _mapper.Map<FamousComments>(famousCommentsDto);
            var CommentPic = HttpContext.Request.Form.Files[0];
            var ProfilePic = HttpContext.Request.Form.Files[1];

            var uploadFileStatus = FileManeger.FileUploader(CommentPic, 1, "FamousCommentImages");
            var uploadFileStatus1 = FileManeger.FileUploader(ProfilePic, 1, "FamousCommentImages");

            if (uploadFileStatus.Status != 200 || uploadFileStatus1.Status != 200) return BadRequest("Internal server error");

            famousComments.CommentPic = uploadFileStatus.Path;
            famousComments.ProfilePic = uploadFileStatus1.Path;
            famousComments.CuserId = ClaimPrincipalFactory.GetUserId(User);
            famousComments.Cdate = DateTime.Now.Ticks;
            _repository.FamousComments.Create(famousComments);



            try
            {
                _repository.Save();
                return Created("", famousComments);
            }
            catch (Exception e)
            {

                FileManeger.FileRemover(new List<string> { uploadFileStatus.Path, uploadFileStatus1.Path });
                return BadRequest(e.Message.ToString());
            }


        }

        [Authorize]
        [HttpDelete]
        [Route("FamousComments/DeleteFamousComments")]
        public IActionResult DeleteFamousComments(long famousCommentId)
        {

            try
            {
                var famousComments = _repository.FamousComments.FindByCondition(c => c.Id.Equals(famousCommentId)).FirstOrDefault();
                if (famousComments == null)
                {

                    return NotFound();

                }
                famousComments.Ddate = DateTime.Now.Ticks;
                famousComments.DuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.FamousComments.Update(famousComments);
                _repository.Save();
                return NoContent();


            }
            catch (Exception e)
            {


                return BadRequest("Internal server error");
            }
        }

        [HttpGet]
        [Route("FamousComments/GetFamousCommentsById")]
        public IActionResult GetFamousCommentsById(long famousCommentId)
        {
            try
            {
                var res = _repository.FamousComments.FindByCondition(c => c.Id == famousCommentId)
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
        [Route("FamousComments/GetFamousCommentsList")]
        public IActionResult GetFamousCommentsList()
        {
            try
            {
                var res = _repository.FamousComments.FindByCondition(c => c.DaUserId == null && c.DuserId == null)
                    .ToList();
         
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest("");
            }
        }

      

        #region UI_Methods


        /// <summary>
        ///لیست نظرات فاخر 
        /// </summary>
        [HttpGet]
        [Route("FamousComments/GetFamousCommentsList_UI")]
        public ListResult<FamousCommentsDto> GetFamousCommentsList_UI()
        {
            try
            {
                var res = _repository.FamousComments.FindByCondition(c => c.DaUserId == null && c.DuserId == null)
                    .ToList();
                var result = _mapper.Map<List<FamousCommentsDto>>(res);

                var finalresult = ListResult<FamousCommentsDto>.GetSuccessfulResult(result);
                return finalresult;

            }
            catch (Exception e)
            {
                return ListResult<FamousCommentsDto>.GetFailResult(null);
            }
        }

        #endregion

    }
}
