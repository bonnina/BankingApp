using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankingApp.Models;
using Microsoft.AspNetCore.Authorization;
using BankingApp.Services;
using BankingApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly BankingAppContext _context;
        private readonly ICheckingAccountService _checkingAccountService;
        private readonly UserManager<BankingAppUser> _userManager;

        public TransactionController(
            BankingAppContext context,
             ICheckingAccountService checkingAccountService, 
             UserManager<BankingAppUser> userManager)
        {
            _context = context;
            _checkingAccountService = checkingAccountService;
            _userManager = userManager;
        }

        // GET: Transaction/Deposit
        public IActionResult Deposit(int CheckingAccountId)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(decimal amount)
        {
            var user = _userManager.FindByIdAsync(User.Identity.Name);
            var userId = _userManager.GetUserId(HttpContext.User);

            var checkingAccount = (await _context.CheckingAccounts
                .FirstAsync(c => c.BankingAppUserId == userId));

            var checkingAccountId = checkingAccount.Id;

            var transaction = new Transaction
            {
                Amount = amount,
                CheckingAccountId = checkingAccountId,
                CheckingAccount = checkingAccount
            };

            if (ModelState.IsValid)
            {
                _context.Transactions.Add(transaction);
                
                await _context.SaveChangesAsync();

                await _checkingAccountService.UpdateBalance(transaction.CheckingAccountId); 

                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}