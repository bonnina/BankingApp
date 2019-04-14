using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.Models
{
    public class CheckingAccount
    {
        public int Id { get; set; }

        [Display(Name = "Account №")]
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Name {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            } }

        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }
    }
}
