using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    class LocationRepository : RepositoryBase<Location>, ILocationRepository
    {
        public LocationRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public List<Location> GetCityList(long provinceId)
        {
            return FindByCondition(c => c.ProvinceId == provinceId && c.DuserId == null && c.DaUserId == null).OrderBy(c => c.Name).ToList();
        }

        public List<Location> GetCountryList()
        {
            return FindByCondition(c => c.Pid == null && c.DuserId == null && c.DaUserId == null).OrderBy(c => c.Name).ToList();
        }

        public List<Location> GetProvinceList(long? countryId)
        {
            var cc = countryId ?? 2775;
            return FindByCondition(c => c.ProvinceId == null && (c.CountryId == cc) && c.DuserId == null && c.DaUserId == null).OrderBy(c => c.Name).ToList();

        }
    }
}
