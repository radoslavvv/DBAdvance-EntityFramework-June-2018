using P01_BillsPaymentSystem.Data;
using P01_BillsPaymentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P01_BillsPaymentSystem.App
{
    class DataBase
    {
        public static void GetUserDetails(int userId, BillsPaymentSystemContext dbContext)
        {
            using (dbContext = new BillsPaymentSystemContext())
            {
                var user = dbContext.Users
                    .Where(u => u.UserId == userId)
                    .Select(x => new
                    {
                        Name = $"{x.FirstName} {x.LastName}",
                        BankAccounts = x.PaymentMethods
                                            .Where(p => p.Type == PaymentMethodType.BankAccount).Select(b => b.BankAccount)
                                            .ToList(),
                        CreditCards = x.PaymentMethods
                                            .Where(p => p.Type == PaymentMethodType.CreaditCard).Select(c => c.CreditCard)
                                            .ToList()
                    }).FirstOrDefault();

                if (user == null)
                {
                    Console.WriteLine($"There is no user with id: {userId}!");
                    return;
                }

                Console.WriteLine($"User: {user.Name}");
                Console.WriteLine($"Bank Accounts: ");
                foreach (var bankAccount in user.BankAccounts)
                {
                    Console.WriteLine($"--ID: {bankAccount.BankAccountId}");
                    Console.WriteLine($"--- Balance: {bankAccount.Balance}");
                    Console.WriteLine($"--- Bank: {bankAccount.BankName}");
                    Console.WriteLine($"--- SWIFT: {bankAccount.SWIFTCode}");
                }

                Console.WriteLine($"Credit Cards: ");
                foreach (var creditCard in user.CreditCards)
                {
                    Console.WriteLine($"--ID: {creditCard.CreditCardId}");
                    Console.WriteLine($"--- Limit: {creditCard.Limit}");
                    Console.WriteLine($"--- Money Owed: {creditCard.MoneyOwed}");
                    Console.WriteLine($"--- Limit Left: {creditCard.LimitLeft}");
                    Console.WriteLine($"--- Expiration Date: {creditCard.ExpirationDate}");
                }
            }
        }

        public static void Seed(BillsPaymentSystemContext dbContext)
        {
            using (dbContext)
            {
                User user = new User
                {
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    Email = "Email1",
                    Password = "Password1"
                };

                CreditCard card = new CreditCard
                {
                    ExpirationDate = DateTime.Now,
                    Limit = 50000,
                    MoneyOwed = 25555550
                };

                BankAccount[] bankAccounts =
                {
                    new BankAccount
                    {
                        Balance= 250,
                        BankName ="Bank1",
                        SWIFTCode = "Code1"
                    },
                    new BankAccount
                    {
                        Balance=255,
                        BankName="Bank2",
                        SWIFTCode="Code2"
                    }
                };

                PaymentMethod[] paymentMethods =
                {
                    new PaymentMethod
                    {
                        User = user,
                        BankAccount = bankAccounts[1],
                        Type = PaymentMethodType.BankAccount
                    },

                     new PaymentMethod
                    {
                        User = user,
                        BankAccount = bankAccounts[0],
                        Type = PaymentMethodType.BankAccount
                    },

                    new PaymentMethod
                    {
                        User = user,
                        CreditCard = card,
                        Type=PaymentMethodType.CreaditCard
                    }
                 };

                dbContext.Users.Add(user);
                dbContext.CreditCards.Add(card);
                dbContext.BankAccounts.AddRange(bankAccounts);
                dbContext.PaymentMethods.AddRange(paymentMethods);

                dbContext.SaveChanges();
            }
        }
    }
}
