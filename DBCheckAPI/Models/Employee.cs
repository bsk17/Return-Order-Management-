using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DBCheckAPI.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        
        [Required]
        public string EmployeeName { get; set; }

        
        public string SSN { get; set; }
    }
}
