using MiniORM.App.Data.Entities;
using System;
using System.Linq;

namespace MiniORM.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var connectionString = @"Server=KING\SQLEXPRESS;Database=MiniORM;Integrated Security=True";

            var context = new SoftUniDbContext(connectionString);

            context.Employees.Add(new Employee
            {
                FirstName = "Gosho",
                LastName = "Inserted",
                DepartmentId = context.Departments.First().Id,
                IsEmployed = true
            });

            var employee = context.Employees.Last();
            employee.FirstName = "Modified";

            context.SaveChanges();
        }
    }
}
