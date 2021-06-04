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
        private string gatewayUrl = "";
        //private ProcessResponse processResponse= new ProcessResponse();

        public ComponentProcessingController(HttpClient client, IConfiguration config)
        {
            this.client = client;
            gatewayUrl = config.GetValue<string>("AppSettings:GatewayComponent");
        }

        [HttpGet]
        public IActionResult CreateProcessRequest()
        {
            var obj = SessionHelper.GetObject<UserRequest>(HttpContext.Session, "CurrentUser");
            if (obj == null)
            {
                return RedirectToAction("Index","Authentication");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProcessRequest(ProcessRequest processRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string stringData = JsonSerializer.Serialize(processRequest);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync($"{gatewayUrl}/ProcessDetails", contentData);

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

                        return RedirectToAction("CompleteProcess", processResponse);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        ViewBag.Message = "400 Credit Card Details Were Wrong";
                        return View("CustomError");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        ViewBag.Message = "No Account found for this Account ID";
                        return View("CustomError");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        ViewBag.Message = "500 Internal Server Error";
                        return View("CustomError");
                    }
                    else
                    {
                        ViewBag.Message = "Server Under Maintenance";
                        return View("CustomError");
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Message = e.Message;
                    return View("CustomError");
                }
            }
            return View("SessionExpired");
        }

      [HttpGet]
      public IActionResult CompleteProcess(ProcessResponse processResponse)
      {
            var obj = SessionHelper.GetObject<UserRequest>(HttpContext.Session, "CurrentUser");
            if (obj == null)
            {
                return RedirectToAction("Index", "Authentication");
            }
            return View(processResponse);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      [ActionName("CompleteProcess")]
      public async Task<IActionResult> CompleteProcessConfirm(ProcessResponse processResponse)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    processResponse.ProcessRequestId = Int32.Parse(TempData["processRequestId"].ToString());
                    processResponse.PackageAndDeliveryCharge = Decimal.Parse(TempData["packageDeliveryCharge"].ToString());
                    processResponse.ProcessingCharge = Decimal.Parse(TempData["processingCharge"].ToString());
                    processResponse.DateOfDelivery = DateTime.Parse(TempData["DateOfDelivery"].ToString());

                    string stringData = JsonSerializer.Serialize(processResponse);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                    string creditCardNumber = TempData["CreditCardNumber"].ToString();

                    HttpResponseMessage response = await client.PostAsync($"{gatewayUrl}/CompleteProcessing/{processResponse.ProcessRequestId}/{creditCardNumber}/{processResponse.ProcessingCharge}", contentData);

                    if (response.IsSuccessStatusCode)
                    {
                        return View("Congratulations");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        ViewBag.Message = "Bad Request Error, Check your card details";
                        return View("CustomError");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        ViewBag.Message = "No Account found for this Account ID";
                        return View("CustomError");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        ViewBag.Message = "500 Internal Server Error";
                        return View("CustomError");
                    }
                    else
                    {
                        ViewBag.Message = "Server Under Maintenance";
                        return View("CustomError");
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Message = e.Message;
                    return View("CustomError");
                }
            }

            return View("SessionExpired");
        }
    }
}
