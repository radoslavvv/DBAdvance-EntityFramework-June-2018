using Microsoft.EntityFrameworkCore;
using P01_BillsPaymentSystem.App;
using P01_BillsPaymentSystem.Data;
using P01_BillsPaymentSystem.Data.Models;
using System;
using System.Linq;

namespace P01_BillsPaymentSystem
{
    public class StartUp
    {
        public static void Main()
        {
            using (var dbContext = new BillsPaymentSystemContext())
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.Migrate();
                DataBase.Seed(dbContext);

                Console.WriteLine("UserId: ");
                int userId = int.Parse(Console.ReadLine());

                DataBase.GetUserDetails(userId, dbContext);

                Console.WriteLine("Withdraw amount: ");
                decimal withdrawAmount = decimal.Parse(Console.ReadLine());

                User user = dbContext
                    .Users
                    .Include(u => u.PaymentMethods)
                    .FirstOrDefault(u => u.UserId == userId);

                var bankAccounts = (from a in dbContext.BankAccounts
                                    from b in user.PaymentMethods
                                    where a.BankAccountId == b.BankAccountId
                                    select a).ToList();

                var creditCards = (from a in dbContext.CreditCards
                                   from b in user.PaymentMethods
                                   where a.CreditCardId == b.CreditCardId
                                   select a).ToList();


                try
                {
                    foreach (var bankAccount in bankAccounts)
                    {
                        bankAccount.Withdraw(withdrawAmount);
                    }

                    foreach (var creditCard in creditCards)
                    {
                        creditCard.Withdraw(withdrawAmount);
                    }

                    dbContext.SaveChanges();
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


    }
}
