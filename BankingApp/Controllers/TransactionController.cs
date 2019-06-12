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
        private readonly ICheckingAccountService _checkingAccountService;

        public TransactionController(
            BankingAppContext context,
             ICheckingAccountService checkingAccountService)
        {
            _context = context;
            _checkingAccountService = checkingAccountService;
        }

        // GET: Transaction/Deposit
        public IActionResult Deposit(int CheckingAccountId)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(Transaction transaction)
        {
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