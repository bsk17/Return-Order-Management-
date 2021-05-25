using ComponentProcessingMicroservice.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComponentProcessingMicroservice.Services
{
    public class RepairProcessCharges : IProcessCharges
    {
        public decimal CalculateProcessCharge()
        {
            return Charges.RepairProcessCharge;
        }
    }
}
