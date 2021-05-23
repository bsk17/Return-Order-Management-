using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Database.Entities
{
    public class CreditCard
    {
        public int CreditCardId { get; set; }

        [Required]
        [MaxLength(50,ErrorMessage ="Name Cannot Exceed 50 characters.")]
        public string Name { get; set; }
        
        [Required]
        public string CreditCardNumber { get; set; }
        
        [Required]
        public decimal CreditCardLimit { get; set; }
    }
}
