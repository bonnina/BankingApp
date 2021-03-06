﻿using Microsoft.EntityFrameworkCore;
using BankingApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Managers
{
    public class CheckingAccountManager: ICheckingAccountManager
    {
        private readonly BankingAppContext _context;

        public CheckingAccountManager(BankingAppContext context)
        {
            _context = context;
        }

        public async Task CreateCheckingAccount(
            string firstName, 
            string lastName, 
            string userId, 
            decimal initialBalance = 0)
        {
            string accountNumber = (11112222 + await _context.CheckingAccounts.CountAsync())
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
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBalance(decimal amount, int checkingAccountId)
        {
            CheckingAccount checkingAccount = await _context.CheckingAccounts
                .FirstAsync(c => c.Id == checkingAccountId);

            checkingAccount.Balance += amount;

            // await _context.SaveChangesAsync();
        }

        public async Task<bool> AccountExists(int checkingAccountId)
        {
            if (!await _context.CheckingAccounts.AnyAsync(c => c.Id == checkingAccountId))
            { 
                return false;
            }

            return true;
        }
    }
}
