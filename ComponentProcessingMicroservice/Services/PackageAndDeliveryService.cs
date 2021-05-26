using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ComponentProcessingMicroservice.Services
{
    public class PackageAndDeliveryService : IPackageAndDeliveryService
    {
        private readonly IConfiguration configuration;
       
        public PackageAndDeliveryService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // this function will get the decimal value from the package and delivery api and provide it to the controller
        public decimal GetPackagingAndDeliveryCharge(string ComponentType, int Count)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration["BaseUrl:PackagingAndDelivery"]);
                    HttpResponseMessage responseMessage 
                        = client.GetAsync($"api/PackagingAndDelivery/{ComponentType}/{Count}").Result;

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var packageAndDeliveryCharge = JsonConvert.DeserializeObject<Decimal>
                            (responseMessage.Content.ReadAsStringAsync().Result);
                        return packageAndDeliveryCharge;
                    }
                    return 0;
                }
            }
            catch (Exception e)
            {
                throw;
            }

        }


    }
}
