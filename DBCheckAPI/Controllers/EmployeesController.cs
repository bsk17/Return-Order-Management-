using DBCheckAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBCheckAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly CheckDbContext _context;
        public EmployeesController(CheckDbContext _context)
        {
            this._context = _context;
        }

        //[HttpGet("{CreditCardNumber}")]
        //public IActionResult GetCreditCard(string CreditCardNumber)
        //{
        //    var creditCardDetails = (from c in _context.CreditCards
        //                             where c.CreditCardNumber == CreditCardNumber
        //                             select c).SingleOrDefault();

        //    return Ok(creditCardDetails);
        //}

        [HttpGet("{SSN}")]
        public IActionResult GetEmployee(string SSN)
        {
            var ssn = SSN;
            try
            {
                var emp = (from e in _context.Employees
                           where e.SSN == ssn
                           select e).SingleOrDefault();
                return Ok(emp);
            }
            catch(Exception e)
            {
                return BadRequest("Exception thrown");
            }
        }

        [HttpPost]
        public IActionResult AddEmployee([FromBody]Employee emp)
        {
            try
            {
                _context.Employees.Add(emp);
                _context.SaveChanges();
                return Ok(emp);
            }
            catch (Exception e)
            {
                return BadRequest("Exception Thrown");
            }
        }
    }
}
