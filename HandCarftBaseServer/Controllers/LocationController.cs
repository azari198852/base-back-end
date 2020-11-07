using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.UIResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HandCarftBaseServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        public IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public LocationController(IMapper mapper, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        #region UI_Methods

        /// <summary>
        ///لیست کشورها 
        /// </summary>
        [HttpGet]
        [Route("Location/GetCountryList_UI")]
        public ListResult<LocationDto> GetCountryList_UI()
        {

            try
            {
                var countrylist = _repository.Location.GetCountryList().ToList();
                var result = _mapper.Map<List<LocationDto>>(countrylist);
                var finalresult = ListResult<LocationDto>.GetSuccessfulResult(result);
                return finalresult;
            }
            catch (Exception e)
            {
                return ListResult<LocationDto>.GetFailResult(null);
            }
        }

        /// <summary>
        ///لیست استان ها براساس آیدی کشور 
        /// </summary>
        [HttpGet]
        [Route("Location/GetProvinceList_UI")]
        public ListResult<LocationDto> GetProvinceList_UI(long? countryId)
        {
            try
            {
                var provincelist = _repository.Location.GetProvinceList(countryId).ToList();
                var result = _mapper.Map<List<LocationDto>>(provincelist);
                var finalresult = ListResult<LocationDto>.GetSuccessfulResult(result);
                return finalresult;

            }
            catch (Exception e)
            {
                return ListResult<LocationDto>.GetFailResult(null);
            }

        }

        /// <summary>
        ///لیست شهرها براساس آیدی استان 
        /// </summary>
        [HttpGet]
        [Route("Location/GetCityList_UI")]
        public ListResult<LocationDto> GetCityList_UI(long provinceId)
        {
            try
            {
                var citylist = _repository.Location.GetCityList(provinceId).ToList();
                var result = _mapper.Map<List<LocationDto>>(citylist);
                var finalresult = ListResult<LocationDto>.GetSuccessfulResult(result);
                return finalresult;
            }
            catch (Exception e)
            {
                return ListResult<LocationDto>.GetFailResult(null);
            }



        }

 
        #endregion
    }
}
