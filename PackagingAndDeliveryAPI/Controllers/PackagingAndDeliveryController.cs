using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackagingAndDeliveryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagingAndDeliveryController : ControllerBase
    {
        // Dictionaries to store the Pricing 
        Dictionary<string, int> PackagingPrice;
        Dictionary<string, int> DeliveryPrice;

        // constructor will inialise the dictionaries with the values 
        public PackagingAndDeliveryController()
        {
            PackagingPrice = new Dictionary<string, int>();
            PackagingPrice.Add("Integral", 100);
            PackagingPrice.Add("Accessory", 50);

            DeliveryPrice = new Dictionary<string, int>();
            DeliveryPrice.Add("Integral", 200);
            DeliveryPrice.Add("Accessory", 100);
        }


        // adding this route to the inititial route will take us to the action - GetPackagingAndDeliveryCharge
        [HttpGet("{ComponentType}/{Count}")]
        public IActionResult GetPackagingAndDeliveryCharge(string ComponentType, int Count)
        {
            // if the component type matches with the requirement specified then perform claculation
            // else throw exception
            try
            {
                var packagingAndDeliveryCharge = 0;
                if (PackagingPrice.ContainsKey(ComponentType) == true && DeliveryPrice.ContainsKey(ComponentType) == true)
                {
                    // packaging price * count + delivery price
                    packagingAndDeliveryCharge = PackagingPrice.GetValueOrDefault(ComponentType) * Count + DeliveryPrice.GetValueOrDefault(ComponentType);
                }
                else
                {
                    throw new Exception("Component type submitted is incorrect.");
                }
                return Ok(packagingAndDeliveryCharge);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error While fetching Packaging and Delivery charges - " + e.Message);
            }
        }
    }
}
