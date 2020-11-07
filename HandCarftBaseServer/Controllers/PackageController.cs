using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.UIResponse;
using HandCarftBaseServer.ServiceProvider.PostService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HandCarftBaseServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        public IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly ILogger<PackageController> _logger;

        public PackageController(IMapper mapper, IRepositoryWrapper repository, ILogger<PackageController> logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }


        /// <summary>
        ///لیست پیکج های فعال به همراه عکس ها 
        /// </summary>
        [HttpGet]
        [Route("Package/GetPackageList_UI")]
        public ListResult<PackageDto> GetPackageList_UI()
        {
            try
            {

                var time = DateTime.Now.Ticks;
                var res = _repository.Package
                    .FindByCondition(c =>
                        c.DaDate == null && c.Ddate == null && c.StartDateTime < time && time < c.EndDateTime)
                    .Include(c => c.PackageImage).ToList();
                var result = _mapper.Map<List<PackageDto>>(res);

                var finalresult = ListResult<PackageDto>.GetSuccessfulResult(result);
                return finalresult;

            }
            catch (Exception e)
            {
                return ListResult<PackageDto>.GetFailResult(null);

            }
        }

        /// <summary>
        ///لیست محصولات پکیج براساس آیدی پکیج 
        /// </summary>
        [HttpGet]
        [Route("Package/GetPackageProductListById_UI")]
        public ListResult<ProductDto> GetPackageProductListById_UI(long packageId)
        {
            try
            {

              
         
                var post = new PostServiceProvider();
                var postpriceparam = new PostGetDeliveryPriceParam
                {
                    Price = 1000000,
                    Weight = 1000,
                    ServiceType = 2,// (int)customerOrder.PostTypeId,
                    ToCityId = 39751
                };
                var postresult = post.GetDeliveryPrice(postpriceparam).Result;


                var res = _repository.PackageProduct
                    .FindByCondition(c => c.PackageId == packageId && c.DaDate == null && c.Ddate == null)
                    .Include(c => c.Product).Select(c => c.Product).ToList();
                var result = _mapper.Map<List<ProductDto>>(res);

                var finalresult = ListResult<ProductDto>.GetSuccessfulResult(result);
                return finalresult;

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message, packageId);
                return ListResult<ProductDto>.GetFailResult(e.Message);

            }
        }

    }
}
