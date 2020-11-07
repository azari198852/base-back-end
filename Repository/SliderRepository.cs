using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
   public class SliderRepository : RepositoryBase<Slider>, ISliderRepository
    {
       public SliderRepository(BaseContext repositoryContext)
           : base(repositoryContext)
       {
       }
    }
}
