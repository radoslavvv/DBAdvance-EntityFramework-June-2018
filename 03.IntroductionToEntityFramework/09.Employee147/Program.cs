using P02_DatabaseFirst.Data;
using System;
using System.Linq;

namespace _09.Employee147
{
    public class Program
    {
        public static void Main()
        {
            using (SoftUniContext dbContext = new SoftUniContext())
            {
                var employee = dbContext.Employees
                    .Select(x => new
                    {
                        EmployeeId = x.EmployeeId,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        JobTitle = x.JobTitle,
                        Projects = x.EmployeesProjects.Select(p => p.Project.Name)
                    }).Where(e => e.EmployeeId == 147)
                      .FirstOrDefault();

                Console.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                foreach (var project in employee.Projects.OrderBy(x => x))
                {
                    Console.WriteLine($"{project}");
                }
            }
        }
    }
}
