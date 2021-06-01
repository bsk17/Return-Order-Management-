using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReturnOrderPortal.Models
{
    public class DefectiveComponent
    {
        public int DefectiveComponentId { get; set; }

        [Required]
        public string ComponentType { get; set; }

        [Required(ErrorMessage ="Enter The Component Name")]
        public string ComponentName { get; set; }

        [Required(ErrorMessage ="Enter the Component Type")]
        public int Quantity { get; set; }
    }
}
