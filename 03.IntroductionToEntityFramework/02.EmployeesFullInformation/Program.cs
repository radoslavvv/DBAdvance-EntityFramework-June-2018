using P02_DatabaseFirst.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntroductionToEntityFramework
{
    public class EmployeesFullInformation
    {
        public static void Main()
        {
            using(SoftUniContext softUniContext = new SoftUniContext())
            {
                List<string> employeesInfo = softUniContext.Employees
                       .OrderBy(e => e.EmployeeId)
                       .Select(e => $"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:0.00}")
                       .ToList();

                foreach (var employee in employeesInfo)
                {
                    Console.WriteLine(employee);
                }
            }
        }
    }
}
