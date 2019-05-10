using System;
using Microsoft.EntityFrameworkCore;
using BankingApp.Models;

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
    }
}
