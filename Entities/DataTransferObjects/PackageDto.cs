using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class PackageDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? Price { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string PackageImageUrl { get; set; }
        public int? ProductCount { get; set; }
        public long? ProductsPriceSum { get; set; }

        private List<PackageImageDto> PackageImage { get; set; }
    }
}
