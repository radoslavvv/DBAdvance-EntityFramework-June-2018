using P02_DatabaseFirst.Data;
using System;
using System.Linq;

namespace _10.DepartmentsWithMoreThan5Emp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (SoftUniContext dbContext = new SoftUniContext())
            {
                var departments = dbContext.Departments
                    .Where(d => d.Employees.Count() > 5)
                    .Select(x => new
                    {
                        EmployeesCount = x.Employees.Count(),
                        Name = x.Name,
                        ManagerName = x.Manager.FirstName + " " + x.Manager.LastName,
                        Employees = x.Employees.Select(e => new
                        {
                            FirstName = e.FirstName,
                            LastName = e.LastName,
                            JobTitle = e.JobTitle
                        })
                    })
                    .OrderBy(d => d.EmployeesCount)
                    .ThenBy(d => d.Name);

                foreach (var department in departments)
                {
                    Console.WriteLine($"{department.Name} - {department.ManagerName}");
                    foreach (var employee in department.Employees.OrderBy(e => e.FirstName).ThenBy(e=>e.LastName))
                    {
                        Console.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                    }

                    Console.WriteLine(new string('-', 10));
                }
            }
        }
    }
}
