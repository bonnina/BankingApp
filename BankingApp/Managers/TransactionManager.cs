using System.Threading.Tasks;
using BankingApp.Models;
using BankingApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BankingApp.Managers
{
    public class TransactionManager: ITransactionManager
    {
        private readonly BankingAppContext _context;
        private readonly ICheckingAccountManager _checkingAccountManager;
        private readonly UserManager<BankingAppUser> _userManager;

        public TransactionManager(
            BankingAppContext context,
            ICheckingAccountManager checkingAccountManager,
            UserManager<BankingAppUser> userManager)
        {
            _context = context;
            _checkingAccountManager = checkingAccountManager;
            _userManager = userManager;
        }

        public async Task CreateTransaction(decimal amount, string userId)
        {
            var checkingAccount = await _context.CheckingAccounts
                .FirstAsync(c => c.BankingAppUserId == userId);

            var checkingAccountId = checkingAccount.Id;

            var transaction = new Transaction
            {
                Amount = amount,
                CheckingAccountId = checkingAccountId,
                CheckingAccount = checkingAccount
            };
            
            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync();

            await _checkingAccountManager.UpdateBalance(transaction.CheckingAccountId);
        }

        public async Task TransferFunds(decimal amount, string userId, int checkingAccountId)
        {
            CheckingAccount senderAccount = await _context.CheckingAccounts
                .FirstAsync(c => c.BankingAppUserId == userId);

            var senderTransaction = new Transaction
            {
                Amount = -System.Math.Abs(amount),
                CheckingAccountId = senderAccount.Id,
                CheckingAccount = senderAccount
            };

            _context.Transactions.Add(senderTransaction);

            senderAccount.Balance = await _context.Transactions
                .Where(c => c.CheckingAccountId == senderAccount.Id)
                .SumAsync(c => c.Amount);
            
            CheckingAccount recipientAccount = await _context.CheckingAccounts
                .FirstAsync(c => c.Id == checkingAccountId);

            var recipientTransaction = new Transaction
            {
                Amount = System.Math.Abs(amount),
                CheckingAccountId = recipientAccount.Id,
                CheckingAccount = recipientAccount
            };

            _context.Transactions.Add(recipientTransaction);

            recipientAccount.Balance = await _context.Transactions
                .Where(c => c.CheckingAccountId == recipientAccount.Id)
                .SumAsync(c => c.Amount);

            await _context.SaveChangesAsync();
        }
    }
}
