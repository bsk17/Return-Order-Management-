using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ComponentProcessingMicroservice.Database.Entities
{
    public class ProcessRequest
    {
        [Key]
        public int ProcessRequestId { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string ContactNumber { get; set; }
        public bool IsPriority { get; set; }


        // details of credit card
        public string CreditCardNumber { get; set; }

        // details of defective component
        // DefectiveComponentId is foreign Key referring to primary key of DefectiveComponent
        public virtual int DefectiveComponentId { get; set; }
        public virtual DefectiveComponent DefectiveComponent { get; set; }
    }
}
