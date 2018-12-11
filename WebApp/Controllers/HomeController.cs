using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    //[Area("Identity")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //var url = Url.Action("Login", "Account");
            //return Content(url);
            return RedirectToAction("Login", "Identity/Account");
            //return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return RedirectToAction("List", "User");
            //return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
