using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.UIResponse;
using Microsoft.Extensions.Logging;

namespace HandCarftBaseServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class WorkController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILogger<ProductController> _logger;

        public WorkController(IMapper mapper, IRepositoryWrapper repository, ILogger<ProductController> logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [Route("Work/GetWorkList")]
        public ListResult<WorkDto> GetWorkList_UI()
        {
            try
            {
                var res = _repository.Work.FindByCondition(c => c.Ddate == null && c.DaDate == null).ToList();
                var result = _mapper.Map<List<WorkDto>>(res);

                var finalresult = ListResult<WorkDto>.GetSuccessfulResult(result);
                return finalresult;

            }
            catch (Exception e)
            {
                return ListResult<WorkDto>.GetFailResult(null);

            }
        }
    }
}
