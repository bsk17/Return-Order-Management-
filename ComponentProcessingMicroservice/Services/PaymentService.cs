using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ComponentProcessingMicroservice.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration configuration;

        public PaymentService(IConfiguration configuration)
        {
            this.configuration = configuration; 
        }

        public bool ProcessPayment(string CreditCardNumber, decimal CreditcardLimit, decimal ProcessingCharge)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration["BaseUrl:Gateway"]);
                    HttpResponseMessage responseMessage = client.GetAsync($"api/Payment/{CreditCardNumber}/{CreditcardLimit}/{ProcessingCharge}").Result;

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
