using System.ComponentModel.DataAnnotations;

namespace BankingApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int CheckingAccountId { get; set; }
        
        public virtual CheckingAccount CheckingAccount { get; set; }
    }
}
