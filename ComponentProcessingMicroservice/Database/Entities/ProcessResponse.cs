using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ComponentProcessingMicroservice.Database.Entities
{
    public class ProcessResponse
    {
        [Key]
        public int ProcessResponsetId { get; set; }
        public decimal ProcessingCharge { get; set; }
        public decimal PackageAndDeliveryCharge { get; set; }
        public DateTime DateOfDelivery { get; set; }

        public virtual int ProcessRequestId { get; set; }
        public virtual ProcessRequest processRequest { get; set; }
    }
}
