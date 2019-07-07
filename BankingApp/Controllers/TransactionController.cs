using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankingApp.Models;
using Microsoft.AspNetCore.Authorization;
using BankingApp.Managers;
using BankingApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace BankingApp.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly BankingAppContext _context;
        private readonly ITransactionManager _transactionManager;
        private readonly UserManager<BankingAppUser> _userManager;

        public TransactionController(
            BankingAppContext context,
             ITransactionManager transactionManager,
             UserManager<BankingAppUser> userManager)
        {
            _context = context;
            _transactionManager = transactionManager;
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

            if (ModelState.IsValid)
            {
                await _transactionManager.CreateTransaction(amount, userId);

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(decimal amount)
        {
            return View();
        }
    }
}