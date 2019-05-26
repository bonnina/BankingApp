using System;
using Microsoft.EntityFrameworkCore;
using BankingApp.Models;
using System.Linq;

namespace BankingApp.Services
{
    public class CheckingAccountService
    {
        private readonly BankingAppContext _context;

        public CheckingAccountService(BankingAppContext context)
        {
            _context = context;
        }

        public async void CreateCheckingAccount(string firstName, string lastName, string userId, 
            decimal initialBalance = 0)
        {
            var accountNumber = (11112222 + await _context.CheckingAccounts.CountAsync())
                           .ToString()
                           .PadLeft(10, '0');

            var checkingAccount = new CheckingAccount
            {
                FirstName = firstName,
                LastName = lastName,
                AccountNumber = accountNumber,
                Balance = initialBalance,
                BankingAppUserId = userId
            };

            _context.CheckingAccounts.Add(checkingAccount);
            _context.SaveChanges();
        }

        public void UpdateBalance(int checkingAccountId)
        {
            var checkingAccount = _context.CheckingAccounts
                .Where(c => c.Id == checkingAccountId)
                .First();

            checkingAccount.Balance = _context.Transactions
                .Where(c => c.Id == checkingAccountId)
                .Sum(c => c.Amount);

            _context.SaveChanges();
        }
    }
}
