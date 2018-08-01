using P02_DatabaseFirst.Data;
using System;
using System.Linq;

namespace _08.AddressesByTown
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (SoftUniContext dbContext = new SoftUniContext())
            {
                var addresses = dbContext.Addresses
                     .Select(x => new
                     {
                         AddressText = x.AddressText,
                         TownName = x.Town.Name,
                         EmployeesCount = x.Employees.Count
                     })
                    .OrderByDescending(a => a.EmployeesCount)
                    .ThenBy(a => a.TownName)
                    .ThenBy(a => a.AddressText)
                    .Take(10);

                foreach (var address in addresses)
                {
                    Console.WriteLine($"{address.AddressText}, {address.TownName} - {address.EmployeesCount} employees");
                }
            }
        }
    }
}
