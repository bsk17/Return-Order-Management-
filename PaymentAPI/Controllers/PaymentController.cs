using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentAPI.Database;
using PaymentAPI.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly CreditCardDbContext _context;

        public PaymentController(CreditCardDbContext _context)
        {
            this._context = _context;
        }

        // we will receive all the three parameters from ComponentProcessingAPI
        [HttpGet("{CreditCardNumber}/{CreditCardLimit}/{ProcessingCharge}")]
        public IActionResult ProcessPayment(string CreditCardNumber,decimal CreditCardLimit, decimal ProcessingCharge)
        {
            try
            {
                //we have to perfrom encryption / decryption of cardnumber

                decimal CurrentBalance = 0;
                var creditCardDetails = (from c in _context.CreditCards
                                         where c.CreditCardNumber == CreditCardNumber
                                         select c).SingleOrDefault();
          
                // check if the credit card limit permits the processing.
                if (ProcessingCharge <= CreditCardLimit && CreditCardLimit == creditCardDetails.CreditCardLimit)
                {
                    CurrentBalance = (creditCardDetails.CreditCardLimit - ProcessingCharge);
                    creditCardDetails.CreditCardLimit = CurrentBalance;
                    _context.SaveChanges();
                    return Ok(CurrentBalance);
                }
                else
                {
                    throw new Exception("You don't have enough limit to proceed with the payment.");
                }

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error While Completing Your Transaction - " + e.Message);
            }
        }
    }
}
