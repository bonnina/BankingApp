﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankingApp.Models;
using Microsoft.AspNetCore.Authorization;
using BankingApp.Managers;
using BankingApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly BankingAppContext _context;
        private readonly ITransactionManager _transactionManager;
        private readonly ICheckingAccountManager _checkingAccountManager;
        private readonly UserManager<BankingAppUser> _userManager;

        public TransactionController(
            BankingAppContext context,
             ITransactionManager transactionManager,
             ICheckingAccountManager checkingAccountManager,
             UserManager<BankingAppUser> userManager)
        {
            _context = context;
            _transactionManager = transactionManager;
            _checkingAccountManager = checkingAccountManager;
            _userManager = userManager;
        }

        // GET: Transaction/Deposit
        public IActionResult Deposit(int checkingAccountId)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(decimal amount)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            if (ModelState.IsValid)
            {
                await _transactionManager.CreateTransaction(amount, userId);

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // GET: Transaction/Withdraw
        public IActionResult Withdraw()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(decimal amount = 100)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            CheckingAccount account = await _context.CheckingAccounts
                .FirstAsync(c => c.BankingAppUserId == userId);

            decimal balance = account.Balance;

            if (amount > balance)
            {
                ViewData["ErrMessage"] = "Insufficient funds";
                return View();
            }

            if (ModelState.IsValid)
            {
                await _transactionManager.CreateTransaction(-System.Math.Abs(amount), userId);

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // GET: Transaction/Transfer
        public IActionResult Transfer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(int checkingAccountId, decimal amount)
        {
            if (!await _checkingAccountManager.AccountExists(checkingAccountId))
            {
                ViewData["ErrMessage"] = "Account number does not exist";
                return View();
            }

            var user = _userManager.FindByIdAsync(User.Identity.Name);
            var userId = _userManager.GetUserId(HttpContext.User);

            CheckingAccount account = await _context.CheckingAccounts
                .FirstAsync(c => c.BankingAppUserId == userId);

            decimal balance = account.Balance;

            if (amount > balance)
            {
                ViewData["ErrMessage"] = "Insufficient funds";
                return View();
            }

            if (ModelState.IsValid)
            {
                await _transactionManager.TransferFunds(amount, account.Id, checkingAccountId);

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Transaction/QuickCash
        public async Task<string> QuickCash()
        {
            decimal amount = 100;
            var user = _userManager.FindByIdAsync(User.Identity.Name);
            var userId = _userManager.GetUserId(HttpContext.User);

            CheckingAccount account = await _context.CheckingAccounts
                .FirstAsync(c => c.BankingAppUserId == userId);

            decimal balance = account.Balance;

            if (amount > balance)
            {
                return "Insufficient funds";
            }

            if (ModelState.IsValid)
            {
                await _transactionManager.CreateTransaction(-amount, userId);

                return "Success!";
            }

            return "Error";
        }
    }
}