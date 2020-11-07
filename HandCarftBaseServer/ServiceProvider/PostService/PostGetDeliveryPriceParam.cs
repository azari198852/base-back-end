using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandCarftBaseServer.ServiceProvider.PostService
{
    public class PostGetDeliveryPriceParam
    {
        public int Price { get; set; }
        public int Weight { get; set; }
        public int ServiceType { get; set; }
        public int ToCityId { get; set; }
        public int PayType { get; set; }
    }
}
