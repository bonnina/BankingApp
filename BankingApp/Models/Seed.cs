using System;
using System.Collections.Generic;
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

        public Seed (UserManager<BankingAppUser> userManager)
        {
             _userManager = userManager;
        }

        public async static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BankingAppContext(
               serviceProvider.GetRequiredService<DbContextOptions<BankingAppContext>>()))
            {
                if (!context.Users.Any(u => u.UserName == "admin@gmail.com"))
                {
                    var user = new BankingAppUser {
                        UserName = "admin@gmail.com",
                        Email = "admin@gmail.com"};

                    _userManager.Create(user, "passW0rd");

                    var service = new CheckingAccountService(context);
                    service.CreateCheckingAccount("admin", "user", user.Id, 1000);

                    context.Roles.Add(new IdentityRole { Name = "Admin" });
                    context.SaveChanges();

                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
