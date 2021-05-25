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
                HttpResponseMessage response = await client.PostAsync(componentProcessingApiUrl, contentData);
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Process Requested successfully";
                }
                else
                {
                    ViewBag.Message = "Error while calling Web API";
                }
            }
            return View();
        }
    }
}
