using System;
using System.Collections.Generic;
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
    public class ParametersController : ControllerBase
    {
        public IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public ParametersController(IMapper mapper, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;
        }


        [Authorize]
        [HttpPost]
        [Route("Parameters/InserParameters")]
        public IActionResult InserParameters(ParametersDto parameters)
        {
            try
            {
                var _Parameters = _mapper.Map<Parameters>(parameters);
                _Parameters.Cdate = DateTime.Now.Ticks;
                _Parameters.CuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.Parameter.Create(_Parameters);



                _repository.Save();
                return Created("", _Parameters);
            }
            catch (Exception e)
            {
                return BadRequest("");
            }


        }

        [Authorize]
        [HttpPut]
        [Route("Parameters/UpdateParameters")]
        public IActionResult UpdateParameters(ParametersDto parameters)
        {
            try
            {
                var _Parameters = _repository.Parameter.FindByCondition(c => c.Id == parameters.Id).FirstOrDefault();
                if (_Parameters == null) return NotFound();
                //  _Parameters.Pid = Parameters.Pid;
                _Parameters.Name = parameters.Name;
                _Parameters.Rkey = parameters.Rkey;
                _Parameters.Mdate = DateTime.Now.Ticks;
                _Parameters.MuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.Parameter.Update(_Parameters);



                _repository.Save();
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest("");
            }


        }

        [Authorize]
        [HttpDelete]
        [Route("Parameters/DeleteParameters")]
        public IActionResult DeleteParameters(long parametersId)
        {
            try
            {
                var _Parameters = _repository.Parameter.FindByCondition(c => c.Id == parametersId).FirstOrDefault();
                if (_Parameters == null) return NotFound();
                _Parameters.Ddate = DateTime.Now.Ticks;
                _Parameters.DuserId = ClaimPrincipalFactory.GetUserId(User);
                _repository.Parameter.Update(_Parameters);



                _repository.Save();
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest("");
            }


        }

        [HttpGet]
        [Route("Parameters/GetParametersById")]
        public IActionResult GetParametersById(long parametersId)
        {
            try
            {
                var res = _repository.Parameter.FindByCondition(c => c.Id == parametersId)
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
        [Route("Parameters/GetParametersList")]
        public IActionResult GetParametersList()
        {

            try
            {
                var list = _repository.Parameter.FindByCondition(c => c.DaDate == null && c.Ddate == null).ToList();
                var fatherlist = list.Where(c => c.Pid == null).ToList();



                var str = "[";

                foreach (var item in fatherlist)
                {
                    str += "{";
                    str += "'mid':" + item.Id + ",";
                    str += "'text':" + "'" + item.Name + "'";
                    str += GetSecondNodes(list, item.Id);
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

        private string GetSecondNodes(List<Parameters> mainlist, long? pid)
        {
            var list = mainlist.Where(c => c.Pid == pid).ToList();
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
                    str += GetSecondNodes(mainlist, item.Id);
                    str += "},";
                }

                str += "]";

            }


            return str;
        }
    }
}
