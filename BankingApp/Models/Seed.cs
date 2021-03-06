﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BankingApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using BankingApp.Managers;

namespace BankingApp.Models
{
    public class Seed
    {
        private readonly UserManager<BankingAppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICheckingAccountManager _checkingAccountManager;
        private readonly BankingAppContext _context;

        public Seed (UserManager<BankingAppUser> userManager,
            ICheckingAccountManager checkingAccountManager,
             BankingAppContext context,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _checkingAccountManager = checkingAccountManager;
            _context = context;
            _roleManager = roleManager;
        }

        public async Task Initialize()
        {
            if (!await _context.Users.AnyAsync(u => u.UserName == "admin@gmail.com"))
            {
                var user = new BankingAppUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    FirstName = "Admin",
                    LastName = "Admin",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                IdentityResult result = await _userManager.CreateAsync(user, "DonaldTrump2016!");
                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(user,
                        new Claim(ClaimTypes.GivenName, user.FirstName));

                    #region Add Admin if not exists

                    if (!await _roleManager.RoleExistsAsync("Admin"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
                    }

                    #endregion

                    await _checkingAccountManager.CreateCheckingAccount(user.FirstName, user.LastName, user.Id, 1000);

                    await _userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    throw new AggregateException(result.Errors.Select(e => new Exception(e.Description)));
                }
            }
        }
    }
}
