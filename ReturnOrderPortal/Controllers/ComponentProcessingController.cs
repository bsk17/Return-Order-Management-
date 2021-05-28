using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReturnOrderPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReturnOrderPortal.Controllers
{
    public class ComponentProcessingController : Controller
    {
        private readonly HttpClient client = null;
        private string componentProcessingApiUrl = "";
        //private ProcessResponse processResponse= new ProcessResponse();

        public ComponentProcessingController(HttpClient client, IConfiguration config)
        {
            this.client = client;
            componentProcessingApiUrl = config.GetValue<string>("AppSettings:ComponentProcessingApiUrl");
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public IActionResult CreateProcessRequest()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProcessRequest(ProcessRequest processRequest)
        {
            if (ModelState.IsValid)
            {
                string stringData = JsonSerializer.Serialize(processRequest);
                var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync($"{componentProcessingApiUrl}/ProcessDetails", contentData);

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Process Requested successfully";
                    TempData["CreditCardNumber"] = processRequest.CreditCardNumber;
                    
                    string stringDataResponse = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    ProcessResponse processResponse = JsonSerializer.Deserialize<ProcessResponse>(stringDataResponse, options);

                    TempData["processingCharge"] = processResponse.ProcessingCharge.ToString();
                    TempData["packageDeliveryCharge"] = processResponse.PackageAndDeliveryCharge.ToString();
                    TempData["DateOfDelivery"] = processResponse.DateOfDelivery.ToString();
                    TempData["processRequestId"] = processResponse.ProcessRequestId.ToString();

                    return RedirectToAction("CompleteProcess",processResponse);
                }
                else
                {
                    ViewBag.Message = "Error while calling Web API";
                }
            }
            return View();
        }

      [HttpGet]
      public IActionResult CompleteProcess(ProcessResponse processResponse)
      {
            return View(processResponse);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      [ActionName("CompleteProcess")]
      public async Task<IActionResult> CompleteProcessConfirm(ProcessResponse processResponse)
        {
            if (ModelState.IsValid)
            {
                processResponse.ProcessRequestId = Int32.Parse(TempData["processRequestId"].ToString());
                processResponse.PackageAndDeliveryCharge = Decimal.Parse(TempData["packageDeliveryCharge"].ToString());
                processResponse.ProcessingCharge = Decimal.Parse(TempData["processingCharge"].ToString());
                processResponse.DateOfDelivery = DateTime.Parse(TempData["DateOfDelivery"].ToString());

                string stringData = JsonSerializer.Serialize(processResponse);
                var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                string creditCardNumber = TempData["CreditCardNumber"].ToString();

                HttpResponseMessage response = await client.PostAsync($"{componentProcessingApiUrl}/CompleteProcessing/{processResponse.ProcessRequestId}/{creditCardNumber}/{processResponse.ProcessingCharge}", contentData);

                if (response.IsSuccessStatusCode)
                {
                    return Content("You have succesfully placed your order. Happy Shopping!!!");
                }
                else
                {
                    return Content("Sorry We couldn't complete your order at the moment!!");
                }

                //return Content(processResponse.ProcessRequestId +" - "+processResponse.ProcessingCharge);
            }

            return View();
        }
    }
}
