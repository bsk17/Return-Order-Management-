using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComponentProcessingMicroservice.Services
{
    public interface IPackageAndDeliveryService
    {
        decimal GetPackagingAndDeliveryCharge(string ComponentType, int Count);
    }
}
