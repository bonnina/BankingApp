using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankingApp.Models;
using Microsoft.AspNetCore.Authorization;
using BankingApp.Services;

namespace BankingApp.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly BankingAppContext _context;

        public TransactionController(BankingAppContext context)
        {
            _context = context;
        }

        // GET: Transaction/Deposit
        public IActionResult Deposit(int CheckingAccountId)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Deposit(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Transactions.Add(transaction);
              //  _context.SaveChanges();

                var service = new CheckingAccountService(_context);
                service.UpdateBalance(transaction.CheckingAccountId);

                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}