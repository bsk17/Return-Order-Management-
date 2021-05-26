using ComponentProcessingMicroservice.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentAPI.Database.Entities;
using ComponentProcessingMicroservice.Database.Entities;
using ComponentProcessingMicroservice.Services;

namespace ComponentProcessingMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentProcessingController : ControllerBase
    {
        private readonly ComponentProcessingDbContext _context;
        private IProcessCharges _processCharges;
        private readonly IPackageAndDeliveryService packageAndDeliveryService;
        private readonly IPaymentService paymentService;
        ProcessResponse processResponse = null;

        public ComponentProcessingController(ComponentProcessingDbContext _context, IProcessCharges _processCharges, IPackageAndDeliveryService packageAndDeliveryService, IPaymentService paymentService, ProcessResponse processResponse)
        {
            this._context = _context;
            this._processCharges = _processCharges;
            this.packageAndDeliveryService = packageAndDeliveryService;
            this.paymentService = paymentService;
            this.processResponse = processResponse;
        }

        [HttpPost("ProcessDetails")]
        public async Task<ActionResult<ProcessResponse>> ProcessDetails([FromBody] ProcessRequest processRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The Process request is having some trouble.");
            }
            else
            {
                // to get the defective component ID we need to save defective component first to the database
                _context.DefectiveComponents.Add(processRequest.DefectiveComponent);
                await _context.SaveChangesAsync();

                // set the defectivve component ID to the property of process request
                var defectiveComponent = processRequest.DefectiveComponent;
                processRequest.DefectiveComponentId = defectiveComponent.DefectiveComponentId;

                // once we set the id then we can proceed to save the process request int he database
                _context.ProcessRequests.Add(processRequest);
                await _context.SaveChangesAsync();

                // CREATING PROCESS RESPONSE TO RETURN

                // Processing Charge for Integral item from RepairProcesscharge Class
                if (processRequest.DefectiveComponent.ComponentType == "Integral")
                {
                    _processCharges = new RepairProcessCharges();

                    if (processRequest.IsPriority == true)
                    {
                        processResponse.ProcessingCharge = (_processCharges.CalculateProcessCharge() + 200) * processRequest.DefectiveComponent.Quantity;
                        processResponse.DateOfDelivery = DateTime.Today.AddDays(2);
                    }
                    else
                    {
                        processResponse.ProcessingCharge = _processCharges.CalculateProcessCharge() * processRequest.DefectiveComponent.Quantity;
                        processResponse.DateOfDelivery = DateTime.Today.AddDays(5);
                    }
                }
                // Processing Charge for Accessory item from ReplaceProcesscharge Class
                else
                {
                    _processCharges = new ReplaceProcessCharges();
                    processResponse.ProcessingCharge = _processCharges.CalculateProcessCharge() * processRequest.DefectiveComponent.Quantity;
                    processResponse.DateOfDelivery = DateTime.Today.AddDays(5);
                }

                processResponse.ProcessRequestId = processRequest.ProcessRequestId;

                // we are calling the Package and Delivery by passing the respective parameters
                decimal packageAndDeliveryCharge = packageAndDeliveryService.GetPackagingAndDeliveryCharge(processRequest.DefectiveComponent.ComponentType, processRequest.DefectiveComponent.Quantity);

                // check for package delivery returned from the api
                if (packageAndDeliveryCharge == 0)
                {
                    return BadRequest("Cannot get Package and Delivery Charge");
                }
                processResponse.PackageAndDeliveryCharge = packageAndDeliveryCharge;

                // return the process response
                return processResponse;
            }
        }
        // 3.1.1 CompletetProcessing POST method which will inturn call 3.1.3 PaymentProcess GET method

        // this method will be called by MVC client using -                                                                                                PostAsync(api/ComponentProcessing/CompleteProcessing/{RequestId}/{CreditCardNumber}/{ProcessingCharge})

        [HttpPost("CompleteProcessing")]
        public async Task<ActionResult> CompleteProcessing(int RequestId, string CreditCardNumber, decimal ProcessingCharge)
        {


            //validate creditcardnumber if null then BadRequest
            var creditCard = (from c in _context.CreditCards
                              where c.CreditCardNumber == CreditCardNumber
                              select c).SingleOrDefault();

            if (creditCard == null)
            {
                return BadRequest("Credit Card details could not be found");
            }

            // to check if the payment was done
            bool paymentComplete = paymentService.ProcessPayment(creditCard.CreditCardNumber, creditCard.CreditCardLimit, ProcessingCharge);

            // saving process response to DB after succesfull transaction
            _context.ProcessResponses.Add(processResponse);
            await _context.SaveChangesAsync();

            // since we are using singleton for processResponse therefore once we add process response to DB
            // we need to reset ProcessResponseId(PrimaryKey to) otherwise DB exception will be thrown
            processResponse.ProcessResponsetId = 0;

            if (paymentComplete != true)
            {
                return BadRequest("Payment Could not be done");
            }

            return Ok("Your request has been processed Successfully. Thankyou for choosing Return Order Portal");

        }
    }
}
