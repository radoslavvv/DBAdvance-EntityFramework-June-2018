﻿using System;
using System.Collections.Generic;
using System.Text;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class BankAccount
    {
        public int BankAccountId { get; set; }

        public decimal Balance { get; set; }

        public string BankName { get; set; }

        public string SWIFTCode { get; set; }

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public void Withdraw(decimal withdrawAmount)
        {
            if (withdrawAmount > this.Balance)
            {
                throw new ArgumentException($"Not enough money to withdraw: {withdrawAmount}");
            }
            this.Balance -= withdrawAmount;
        }

        public void Deposit(decimal depositMoney)
        {
            if (depositMoney < 0)
            {
                throw new ArgumentException("Deposit money value can't be less than zero!");
            }

            this.Balance += depositMoney;
        }
    }
}
