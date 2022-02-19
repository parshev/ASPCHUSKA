using ASPChushka.Data;
using ASPChushka.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ASPChushka.Controllers
{
    public class HomeController : Controller
    {
        private ChushkaContext context;

        public HomeController(ChushkaContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            context.Users.Add(new User() { 
                FullName = "Dsad",
                Username = "Dadada",
                Password = "dsf"
            });
            context.SaveChanges();
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
