using ComponentProcessingMicroservice.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentAPI.Database.Entities;
using ComponentProcessingMicroservice.Database.Entities;

namespace ComponentProcessingMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentProcessingController : ControllerBase
    {
        private readonly ComponentProcessingDbContext _context;
        public ComponentProcessingController(ComponentProcessingDbContext _context)
        {
            this._context = _context;
        }

        [HttpPost]
        public async Task<ActionResult<ProcessResponse>> ProcessDetails([FromBody]ProcessRequest processRequest)
        {
            var processRespone = new ProcessResponse() ;

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

                // creating the object of process response
                processRespone.ProcessRequestId = processRequest.ProcessRequestId;
                processRespone.ProcessingCharge = (decimal)500.00;
                processRespone.PackageAndDeliveryCharge = (decimal)200.00;
                processRespone.DateOfDelivery = new DateTime(2021, 6, 20);

                // return the process response
                return processRespone;
            }
        }
    }
}
