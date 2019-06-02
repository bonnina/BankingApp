using System.Threading.Tasks;

namespace BankingApp.Services
{
    public interface ICheckingAccountService
    {
        Task CreateCheckingAccount(string firstName, string lastName, string userId, decimal initialBalance = 0);

        Task UpdateBalance(int checkingAccountId);
    }
}
