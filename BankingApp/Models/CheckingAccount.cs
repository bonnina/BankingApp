﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using BankingApp.Areas.Identity.Data;

namespace BankingApp.Models
{
    public class CheckingAccount
    {
        [Required]
        [StringLength(10)]
      //  [RegularExpression(@"\d{6,10}", ErrorMessage = "Account number should be between 6 and 10 digits")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Account №")]
        public string AccountNumber { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Name {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            } }

        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        public virtual BankingAppUser User { get; set; }

        [Required]
        public string BankingAppUserId { get; set; }
    }
}
