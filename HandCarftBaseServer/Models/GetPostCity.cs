using System;
using System.Collections.Generic;

namespace HandCarftBaseServer.Models
{
    public partial class GetPostCity
    {
        public long Id { get; set; }
        public int? StateId { get; set; }
        public int? CityCode { get; set; }
        public string Name { get; set; }
    }
}
