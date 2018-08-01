using P02_DatabaseFirst.Data;
using System;
using System.Linq;

namespace _13.FindEmployeesByFirstNameSa
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (SoftUniContext dbContext = new SoftUniContext())
            {
                var employees = dbContext.Employees.Where(e => e.FirstName.StartsWith("Sa"))
                    .Select(x => new { FirstName = x.FirstName, LastName = x.LastName, JobTitle = x.JobTitle, Salary = x.Salary });


                foreach (var employee in employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
                {
                    Console.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:0.00})");
                }
            }
        }
    }
}
