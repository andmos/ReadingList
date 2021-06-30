using System;
using System.Threading;
using System.Threading.Tasks;
using Carter;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ReadingList.Carter
{
    public class NewtonsoftJsonResponseNegotiatorWithEnumConverter : IResponseNegotiator
    {
        private readonly JsonSerializerSettings jsonSettings;

        public NewtonsoftJsonResponseNegotiatorWithEnumConverter()
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            this.jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver, 
                NullValueHandling = NullValueHandling.Ignore, 
                Converters = new JsonConverter[]
                {
                    new Newtonsoft.Json.Converters.StringEnumConverter()
                }
            };
        }
        public bool CanHandle(MediaTypeHeaderValue accept)
        {
            return accept.MediaType.ToString().IndexOf("json", StringComparison.OrdinalIgnoreCase) >= 0;
        }
        public Task Handle(HttpRequest req, HttpResponse res, object model, CancellationToken cancellationToken)
        {
            res.ContentType = "application/json; charset=utf-8";
            return res.WriteAsync(JsonConvert.SerializeObject(model, this.jsonSettings), cancellationToken);
        }
    }
}