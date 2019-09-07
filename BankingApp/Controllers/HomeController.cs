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
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<BankingAppUser> _userManager;
        private readonly BankingAppContext _context;

        public HomeController(BankingAppContext context, UserManager<BankingAppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
          //  var user = _userManager.FindByIdAsync(User.Identity.Name);
            var userId = _userManager.GetUserId(HttpContext.User);

            var checkingAccountId = (await _context.CheckingAccounts
                .FirstAsync(c => c.BankingAppUserId == userId))
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

            return PartialView("_ContactResponse");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
