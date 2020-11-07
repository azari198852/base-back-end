using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class SliderPlaceRepository : RepositoryBase<SliderPlace>, ISliderPlaceRepository
    {
        public SliderPlaceRepository(BaseContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
