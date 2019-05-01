using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankingApp.Models;
using BankingApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BankingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<BankingAppUser> _userManager;
        private readonly BankingAppContext _context;

        [Authorize]
        public IActionResult Index()
        {
            var user = _userManager.FindByIdAsync(User.Identity.Name);
            var userId = _userManager.GetUserId(HttpContext.User);

            var checkingAccountId = _context.CheckingAccounts
                .Where(c => c.BankingAppUserId == userId)
                .First()
                .Id;

            ViewData["CheckingAccountId"] = checkingAccountId;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Send us a message";

            return View();
        }

        [HttpPost]
        public IActionResult Contact(string message)
        {
            ViewData["Message"] = "Thanks, your message was sent.";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
