using System.Threading.Tasks;

namespace BankingApp.Managers
{
    public interface ICheckingAccountManager
    {
        Task CreateCheckingAccount(string firstName, string lastName, string userId, decimal initialBalance = 0);

        Task UpdateBalance(int checkingAccountId);

        Task<bool> AccountExists(int checkingAccountId);
    }
}
