using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Contracts;
using RestSharp;

namespace Repository
{
    public class SyncDataBaseService : ISyncDataBaseService
    {
        private readonly IRepositoryWrapper _repository;

        public SyncDataBaseService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
        public void Sync<T>(string tableName, int serviceType, T entity)
        {
            var syncservice = _repository.TablesServiceDiscovery
                .FindByCondition(c => c.Tables.Name == tableName && c.ServiceType == serviceType).ToList();

            syncservice.ForEach(c =>
            {

                var body = JsonSerializer.Serialize(entity);
                var client = new RestClient(c.Url);
                switch (serviceType)
                {
                    case 1:
                        var postrequest = new RestRequest(Method.POST);
                        postrequest.AddJsonBody(body);
                        IRestResponse postresponse = client.Execute(postrequest);
                        break;
                    case 2:
                        var putrequest = new RestRequest(Method.PUT);
                        putrequest.AddJsonBody(body);
                        IRestResponse putresponse = client.Execute(putrequest);
                        break;
                    case 3:
                        var deleterequest = new RestRequest(Method.PUT);
                        deleterequest.AddJsonBody(body);
                        IRestResponse deleteresponse = client.Execute(deleterequest);
                        break;
                }
                

            });
        }
    }
}
