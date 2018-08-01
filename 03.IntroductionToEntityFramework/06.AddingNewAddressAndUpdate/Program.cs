using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _06.AddingNewAddressAndUpdate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Address address = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            using (SoftUniContext dbContext = new SoftUniContext())
            {
                Employee employee = dbContext.Employees
                    .Where(e => e.LastName == "Nakov")
                    .FirstOrDefault();

                employee.Address = address;

                dbContext.Update(employee);
                dbContext.SaveChanges();

                List<string> addresses = dbContext.Employees
                    .OrderByDescending(e => e.AddressId)
                    .Take(10)
                    .Select(e => e.Address.AddressText)
                    .ToList();

                foreach (var addressText in addresses)
                {
                    Console.WriteLine(addressText);
                }
            }
        }
    }
}
