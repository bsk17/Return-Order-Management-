using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ComponentProcessingMicroservice.Database.Entities
{
    public class DefectiveComponent
    {
        public int DefectiveComponentId { get; set; }

        [Required]
        public string ComponentType { get; set; }

        [Required]
        public string ComponentName { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
