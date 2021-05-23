using ComponentProcessingMicroservice.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentAPI.Database.Entities;

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

        [HttpGet]
        public ProcessResponse ProcessDetail([FromBody]ProcessRequest processRequest )
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest("Unable to fetch data.."+e.Message);
            }
        }
    }
}
