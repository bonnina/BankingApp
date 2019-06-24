using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BankingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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

        // GET: CheckingAccount
        public ActionResult Index()
        {
            return View();
        }

        // GET: CheckingAccount/Details/5
        public ActionResult Details()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var checkingAccount = _context.CheckingAccounts.Where(c => c.BankingAppUserId == userId).First();

            return View(checkingAccount);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DetailsForAdmin(int id)
        {
            var checkingAccount = _context.CheckingAccounts.Find(id);

            return View("Details", checkingAccount);
        }

        // GET: CheckingAccount/List
        [Authorize(Roles = "Admin")]
        public ActionResult List()
        {
            return View(_context.CheckingAccounts.ToList());
        }

        public ActionResult Statement(int id)
        {
            var checkingAccount = _context.CheckingAccounts.Find(id);

            return View(checkingAccount.Transactions.ToList());
        }

        // GET: CheckingAccount/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CheckingAccount/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CheckingAccount/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CheckingAccount/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CheckingAccount/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CheckingAccount/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}