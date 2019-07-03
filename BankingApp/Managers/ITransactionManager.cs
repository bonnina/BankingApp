using System.Threading.Tasks;

namespace BankingApp.Managers
{
    public interface ITransactionManager
    {
        Task CreateTransaction(decimal amount, string userId);
    }
}
