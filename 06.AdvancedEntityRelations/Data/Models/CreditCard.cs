using System;
using System.Collections.Generic;
using System.Text;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class CreditCard
    {
        public int CreditCardId { get; set; }

        public decimal Limit { get; set; }

        public decimal MoneyOwed { get; set; }

        public decimal LimitLeft => this.Limit - this.MoneyOwed;

        public DateTime ExpirationDate { get; set; }

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public void Withdraw(decimal withdrawAmount)
        {
            if (withdrawAmount > this.Limit)
            {
                throw new ArgumentException($"Not enough money to withdraw: {withdrawAmount}");
            }
            this.MoneyOwed += withdrawAmount;
        }

        public void Deposit(decimal depositMoney)
        {
            if (depositMoney < 0)
            {
                throw new ArgumentException("Deposit money value can't be less than zero!");
            }

            this.MoneyOwed -= depositMoney;
        }
    }
}
