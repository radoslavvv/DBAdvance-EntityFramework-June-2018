using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data;
using System;
using System.Globalization;
using System.Linq;

namespace _04.EmployeesAndProjects
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (SoftUniContext dbContext = new SoftUniContext())
            {
                var employees = dbContext.Employees
                    .Where(e => e.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003))
                    .Select(x => new
                    {
                        EmployeeName = x.FirstName + " " + x.LastName,
                        ManagerName = x.Manager.FirstName + " " + x.Manager.LastName,
                        Projects = x.EmployeesProjects
                            .Select(p => new
                            {
                                ProjectName = p.Project.Name,
                                ProjectStartDate = p.Project.StartDate,
                                ProjectEndDate = p.Project.EndDate
                            }).ToList()
                    }).Take(30).ToList();

                foreach (var employee in employees)
                {
                    Console.WriteLine($"{employee.EmployeeName} - Manager: {employee.ManagerName}");

                    foreach (var project in employee.Projects)
                    {
                        Console.WriteLine($"--{project.ProjectName}" +
                            $" - {project.ProjectStartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {project.ProjectEndDate?.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) ?? "not finished"}");
                    }
                }
            }
        }
    }
}
