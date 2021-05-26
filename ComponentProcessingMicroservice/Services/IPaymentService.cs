using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComponentProcessingMicroservice.Services
{
    public interface IPaymentService
    {
        bool ProcessPayment(string CreditCardNumber,decimal CreditCardLimit,decimal ProcessingCharge);
    }
}
