using P02_DatabaseFirst.Data;
using System;
using System.Linq;

namespace _12.IncreaseSalaries
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (SoftUniContext dbContext = new SoftUniContext())
            {
                var employeesToBeRaised = dbContext.Employees
                  .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design" || e.Department.Name == "Marketing" || e.Department.Name == "Information Services");


                foreach (var employee in employeesToBeRaised)
                {
                    employee.Salary *= 1.12m;
                }
                dbContext.SaveChanges();

                foreach (var employee in employeesToBeRaised.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
                {
                    Console.WriteLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:0.00})");
                }
            }
        }
    }
}
