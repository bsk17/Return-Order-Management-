using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ReturnOrderPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ReturnOrderPortal.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly HttpClient client = null;
        private string gatewayUrl = "";
        public AuthenticationController(HttpClient client, IConfiguration config)
        {
            this.client = client;
            gatewayUrl = config.GetValue<string>("AppSettings:GatewayAuth");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var obj = SessionHelper.GetObject<UserRequest>(HttpContext.Session, "CurrentUser");
            
            if (obj != null) 
                return RedirectToAction("HomePage");
           
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IFormCollection form)
        {
            UserRequest userRequest = new UserRequest()
            {
                Email = form["mail"],
                Password = form["pass"]
            };

            if (!ModelState.IsValid)
                return View("Error");

            var jsonstring = JsonConvert.SerializeObject(userRequest);
            var obj = new StringContent(jsonstring, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{gatewayUrl}/Login", obj);
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                UserResponse resobj = 
                    JsonConvert.DeserializeObject<UserResponse>(response.Content.ReadAsStringAsync().Result);
                HttpContext.Session.SetString("Token", resobj.Token);//Set
                string token = HttpContext.Session.GetString("Token");//Get Token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                if (resobj.Token != null)
                {
                    HttpContext.Session.SetInt32("UserId", resobj.Id);
                    HttpContext.Session.SetString("Message", resobj.Message);
                    SessionHelper.SetObject(HttpContext.Session, "CurrentUser", userRequest);
                    return RedirectToAction("HomePage", "Authentication");
                }
                else
                {
                    ViewBag.Message = "Token Expired";
                    return View("CustomError");
                }
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                ModelState.AddModelError("", "Bad credential.");
                ViewBag.Message = "Bad credential.";
                return View("CustomError");
            }
            else
            {
                return View("Invalid");
            }
        }

        public IActionResult HomePage()
        {
            var obj = SessionHelper.GetObject<UserRequest>(HttpContext.Session, "CurrentUser");
            if (obj != null)
            {
                ViewBag.Id = HttpContext.Session.GetInt32("UserId");
                //return View(SessionHelper.GetObject<UserRequest>(HttpContext.Session, "CurrentUser"));
                return Content("You have logged in and your jwt token is - "+ HttpContext.Session.GetString("Token"));
            }
            else
                return View("Index");

        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("CurrentUser");
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
