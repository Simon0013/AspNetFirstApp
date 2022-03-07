using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Registration()
        {
            return View("~/Views/Database/Registration.cshtml");
        }

        public IActionResult Autorisation()
        {
            return View("~/Views/Database/Autorisation.cshtml");
        }

        public IActionResult AddBook()
        {
            return View("~/Views/Database/AddBook.cshtml");
        }

        public IActionResult CreateDiscussion()
        {
            return View("~/Views/Database/CreateDiscussion.cshtml");
        }

        public IActionResult AddComment()
        {
            return View("~/Views/Database/AddComment.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
