using System.Threading.Tasks;

namespace BankingApp.Managers
{
    public interface ITransactionManager
    {
        Task CreateTransaction(decimal amount, string userId);

        Task PrepareTransaction(decimal amount, int checkingAccountId);

        Task TransferFunds(decimal amount, int senderAccountId, int recipientAccountId);
    }
}
