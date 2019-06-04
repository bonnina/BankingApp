using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BankingApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using BankingApp.Services;

namespace BankingApp.Models
{
    public class Seed
    {
        private readonly UserManager<BankingAppUser> _userManager;
        private readonly ICheckingAccountService _checkingAccountService;

        public Seed (UserManager<BankingAppUser> userManager, ICheckingAccountService checkingAccountService)
        {
            _userManager = userManager;
            _checkingAccountService = checkingAccountService;
        }

        public async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BankingAppContext(
                serviceProvider.GetRequiredService<DbContextOptions<BankingAppContext>>()))
            {
                if (!context.Users.Any(u => u.UserName == "admin@gmail.com"))
                {
                    var user = new BankingAppUser
                    {
                        UserName = "admin@gmail.com",
                        Email = "admin@gmail.com"
                    };

                    await _userManager.CreateAsync(user, "passW0rd");

                    await _checkingAccountService.CreateCheckingAccount("admin", "user", user.Id, 1000);

                    context.Roles.Add(new IdentityRole {Name = "Admin"});
                    context.SaveChanges();

                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
