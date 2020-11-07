using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects
{
   public class SliderDto
    {
        public long Id { get; set; }
        public long? SliderPlaceId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string ImageHurl { get; set; }
        public int? Rorder { get; set; }
        public string LinkUrl { get; set; }
    }
}
