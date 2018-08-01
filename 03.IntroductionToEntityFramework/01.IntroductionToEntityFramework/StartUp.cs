using System;
using System.Collections.Generic;
using System.Linq;

using P02_DatabaseFirst;
using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace P02_DatabaseFirst
{
    public class Startup
    {
        public static void Main()
        {
            //SoftUniContext softUniContext = new SoftUniContext();
            //List<string> employeesInfo = softUniContext.Employees
            //    .OrderBy(e => e.EmployeeId)
            //    .Select(e => $"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:0.00}")
            //    .ToList();

            //foreach (var employee in employeesInfo)
            //{
            //    Console.WriteLine(employee);
            //}

            using(SoftUniContext context = new SoftUniContext())
            {
                var employees = context.Employees.ToArray();

                foreach (var employee in employees)
                {
                    Console.WriteLine(employee.FirstName);
                }
            }
        }
    }
}
