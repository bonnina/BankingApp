using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the BankingAppUser class
    public class BankingAppUser : IdentityUser
    {
        [PersonalData]
        [Required]
        public string FirstName { get; set; }

        [PersonalData]
        [Required]
        public string LastName { get; set; }
    }
}
