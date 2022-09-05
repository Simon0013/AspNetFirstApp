using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
        private ApplicationContext db;
        public HomeController(ApplicationContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string email = User.Identity.Name;
                User userFound = db.Users.FirstOrDefault(u => u.Email == email);
                if (userFound == null)
                {
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Registration()
        {
            if (User.Identity.IsAuthenticated)
                return View("AlreadyAuthenticated");
            return View("~/Views/Database/Registration.cshtml");
        }

        public IActionResult Autorisation()
        {
            if (User.Identity.IsAuthenticated)
                return View("AlreadyAuthenticated");
            return View("~/Views/Database/Autorisation.cshtml");
        }

        [Authorize]
        public IActionResult DelAccount()
        {
            return View("~/Views/Database/DelAccount.cshtml");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Autorisation", "Home");
        }

        public IActionResult Book()
        {
            return View("~/Views/Database/Books.cshtml", db);
        }

        public IActionResult Discussion()
        {
            return View("~/Views/Database/Discussions.cshtml", db);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
