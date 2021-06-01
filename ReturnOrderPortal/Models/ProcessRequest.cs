using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReturnOrderPortal.Models
{
    public class ProcessRequest
    {
        [Key]
        public int ProcessRequestId { get; set; }

        [Required]
        
        public string Name { get; set; }

        [Required]
        [Display(Name="Contact Number")]
        public string ContactNumber { get; set; }
        
        // details of credit card
        [Required]
        [StringLength(16,ErrorMessage ="Credit Card Number should be of 16 digits")]
        [Display(Name ="Credit Card Number")]
        public string CreditCardNumber { get; set; }

        public bool IsPriority { get; set; }

        // details of defective component
        // DefectiveComponentId is foreign Key referring to primary key of DefectiveComponent
        public virtual int DefectiveComponentId { get; set; }
        public virtual DefectiveComponent DefectiveComponent { get; set; }
    }
}
