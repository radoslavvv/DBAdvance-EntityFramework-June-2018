using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _02.EmployeesWithSalaryOver50_000
{
    public class Program
    {
        public static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                List<string> employeeNames = context.Employees
                    .Where(e => e.Salary > 50_000)
                    .OrderBy(e => e.FirstName)
                    .Select(e=>e.FirstName)
                    .ToList();

                foreach (var employeeName in employeeNames)
                {
                    Console.WriteLine(employeeName);
                }
            }
        }
    }
}
