using BankingApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Models
{
    public class BankingAppContext : IdentityDbContext<BankingAppUser>
    {
        public BankingAppContext()
        {
        }

        public BankingAppContext(DbContextOptions<BankingAppContext> options)
            : base(options)
        {
        }

        public DbSet<CheckingAccount> CheckingAccounts { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
    }
}
