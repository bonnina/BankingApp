using System.Threading.Tasks;
using BankingApp.Models;
using BankingApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Managers
{
    public class TransactionManager: ITransactionManager
    {
        private readonly BankingAppContext _context;
        private readonly ICheckingAccountManager _checkingAccountManager;

        public TransactionManager(
            BankingAppContext context,
            ICheckingAccountManager checkingAccountManager,
            UserManager<BankingAppUser> userManager)
        {
            _context = context;
            _checkingAccountManager = checkingAccountManager;
        }

        public async Task CreateTransaction(decimal amount, string userId)
        {
            CheckingAccount checkingAccount = await _context.CheckingAccounts
                .FirstAsync(c => c.BankingAppUserId == userId);

            await this.PrepareTransaction(amount, checkingAccount.Id);

            await _checkingAccountManager.UpdateBalance(amount, checkingAccount.Id);

            await _context.SaveChangesAsync();
        }

        public async Task PrepareTransaction(decimal amount, int checkingAccountId)
        {
            CheckingAccount account = await _context.CheckingAccounts
                .FirstAsync(c => c.Id == checkingAccountId);

            Transaction transaction = new Transaction
            {
                Amount = amount,
                CheckingAccountId = account.Id,
                CheckingAccount = account
            };

            _context.Transactions.Add(transaction);
        }

        public async Task TransferFunds(decimal amount, int senderAccountId, int recipientAccountId)
        {
            decimal positiveAmount = System.Math.Abs(amount);
            decimal negativeAmount = -System.Math.Abs(amount);

            await this.PrepareTransaction(negativeAmount, senderAccountId);
            await _checkingAccountManager.UpdateBalance(negativeAmount, senderAccountId);

            await this.PrepareTransaction(positiveAmount, recipientAccountId);
            await _checkingAccountManager.UpdateBalance(positiveAmount, recipientAccountId);

            await _context.SaveChangesAsync();
        }
    }
}
