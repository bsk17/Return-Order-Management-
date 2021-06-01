using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReturnOrderPortal.Models
{
    public class UserRequest
    {
        [Required]
        [EmailAddress(ErrorMessage ="Please enter valid email!")]
        public string Email { get; set; }
        [Required]
        [MinLength(8), MaxLength(16)]

        public string Password { get; set; }
    }
}
