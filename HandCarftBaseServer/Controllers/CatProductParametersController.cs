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
using Microsoft.Extensions.Logging;

namespace HandCarftBaseServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CatProductParametersController : ControllerBase
    {
        public IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILogger<CatProductParametersController> _logger;

        public CatProductParametersController(IMapper mapper, IRepositoryWrapper repository, ILogger<CatProductParametersController> logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }

        //  [Authorize]
        [HttpPut]
        [Route("CatProductParameters/UpdateCatProductParameters")]
        public IActionResult InsertCatProductParameters(long catProductId, List<long> parametersIdList)
        {


            try
            {
                var deletedList = _repository.CatProductParameters.FindByCondition(c => c.CatProductId == catProductId && !parametersIdList.Contains(c.ParametersId.Value))
                    .ToList();
                _repository.CatProductParameters.RemoveRange(deletedList);

                var indbIDs = _repository.CatProductParameters.FindByCondition(c => c.CatProductId == catProductId && c.DaDate == null && c.Ddate == null)
                    .Select(c => c.ParametersId.Value).ToList();

                var tobeInsertedList = parametersIdList.Except(indbIDs).ToList();

                tobeInsertedList.ForEach(c =>
                {
                    CatProductParameters catp = new CatProductParameters
                    {
                        CatProductId = catProductId,
                        ParametersId = c,
                        Cdate = DateTime.Now.Ticks,
                        CuserId = ClaimPrincipalFactory.GetUserId(User)
                    };


                    var productlist = _repository.Product.FindByCondition(c => c.CatProductId == catProductId)
                        .Select(c => c.Id).ToList();
                    productlist.ForEach(c =>
                    {
                        var productparams = new ProductCatProductParameters
                        {
                            Value = null,
                            Cdate = DateTime.Now.Ticks,
                            CuserId = ClaimPrincipalFactory.GetUserId(User),
                            ProductId = c,

                        };
                        catp.ProductCatProductParameters.Add(productparams);
                    });
                    _repository.CatProductParameters.Create(catp);
                });

                _repository.Save();
                return Ok("");

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest("Internal Server Error");
            }
        }

        [HttpGet]
        [Route("CatProductParameters/GetCatProductParametersByCatId")]
        public IActionResult GetCatProductParametersByCatId(long catProductId)
        {

            try
            {

                var selectedList = _repository.CatProductParameters.FindByCondition(c => c.CatProductId == catProductId && c.ParametersId != null && c.Ddate == null && c.DaDate == null)
                    .Select(c => c.ParametersId.Value).ToList();


                return Ok(selectedList);
            }
            catch (Exception e)
            {
                return BadRequest("Internal Server Error");
            }
        }


        [HttpGet]
        [Route("CatProductParameters/GetCatProductParametersTreeByCatId")]
        public IActionResult GetCatProductParametersTreeByCatId(long catProductId)
        {

            try
            {
                var list = _repository.Parameter.FindByCondition(c => c.DaDate == null && c.Ddate == null).ToList();
                var fatherlist = list.Where(c => c.Pid == null).ToList();

                var selectedList = _repository.CatProductParameters.FindByCondition(c => c.CatProductId == catProductId && c.ParametersId != null && c.Ddate == null && c.DaDate == null)
                    .Select(c => c.ParametersId.Value).ToList();

                var str = "[";

                foreach (var item in fatherlist)
                {
                    str += "{";
                    str += "'mid':" + item.Id + ",";
                    str += "'text':" + "'" + item.Name + "'";
                    if (selectedList.Any(c => c == item.Id))
                    {

                        str += ",'state':{'checked': true}";
                    }
                    str += GetSecondNodes(list, selectedList, item.Id);
                    str += "},";
                }

                str += "]";


                return Ok(str);
            }
            catch (Exception e)
            {
                return BadRequest("Internal Server Error");
            }
        }

        private string GetSecondNodes(List<Parameters> mainlist, List<long> selectedList, long? pid)
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
                    if (selectedList.Any(c => c == item.Id))
                    {
                        str += ",'state':{'checked': true}";
                    }
                    str += GetSecondNodes(mainlist, selectedList, item.Id);
                    str += "},";
                }

                str += "]";

            }


            return str;
        }
    }
}
