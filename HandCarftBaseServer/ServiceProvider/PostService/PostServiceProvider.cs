using PostService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Security;
using System.Threading.Tasks;

namespace HandCarftBaseServer.ServiceProvider.PostService
{
    public class PostServiceProvider
    {
        private readonly EshopServiceClient eshopsvc;
        private string UserName = "sanaydasti411";
        private string PassWord = "gI2DuWLuPEz7";

        public PostServiceProvider()
        {
            this.eshopsvc = new EshopServiceClient();
        }


        public Task<List<State>> GetStates()
        {

            var a = eshopsvc.GetStatesAsync(UserName, PassWord);
            return a;
        }

        public  Task<List<City>> GetCities(int stateId)
        {
            var a =  eshopsvc.GetCitiesAsync(UserName, PassWord, stateId);
            return a;

        }

        public Task<DeliveryPrice> GetDeliveryPrice(PostGetDeliveryPriceParam delivery)
        {var a = eshopsvc.GetDeliveryPriceAsync(UserName, PassWord, 51, 380000, 400, 1, 1, 41);
            //var a = eshopsvc.GetDeliveryPriceAsync(UserName, PassWord, 91, delivery.Price, delivery.Weight, delivery.ServiceType,88,delivery.ToCityId);
            return a;
        }


    }
}
