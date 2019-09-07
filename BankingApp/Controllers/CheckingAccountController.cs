using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankingApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Controllers
{
    [Authorize]
    public class CheckingAccountController : Controller
    {
        private readonly BankingAppContext _context;

        public CheckingAccountController(BankingAppContext context)
        {
            _context = context;
        }

        // GET: CheckingAccount/Details/5
        public async Task<ActionResult> Balance()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            CheckingAccount checkingAccount = await _context.CheckingAccounts.FirstAsync(c => c.BankingAppUserId == userId);

            return View(checkingAccount);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DetailsForAdmin(int id)
        {
            CheckingAccount checkingAccount = _context.CheckingAccounts.Find(id);

            return View("Balance", checkingAccount);
        }

        // GET: CheckingAccount/List
        [Authorize(Roles = "Admin")]
        public ActionResult List()
        {
            return View(_context.CheckingAccounts.ToList());
        }

        public ActionResult Statement()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            CheckingAccount checkingAccount = _context.CheckingAccounts
                .Include(c => c.Transactions)
                .First(c => c.BankingAppUserId == userId);

            return View(checkingAccount.Transactions.ToList());
        }
    }
}