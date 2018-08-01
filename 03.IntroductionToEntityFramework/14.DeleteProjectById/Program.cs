using P02_DatabaseFirst.Data;
using System;
using System.Linq;

namespace _14.DeleteProjectById
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (SoftUniContext dbContext = new SoftUniContext())
            {
                var projectToBeRemoved = dbContext.Projects.Where(p => p.ProjectId == 2).FirstOrDefault();

                var employeesRelatedToTheProjects = dbContext.EmployeesProjects.Where(e => e.ProjectId == 2);

                dbContext.RemoveRange(employeesRelatedToTheProjects);
                dbContext.Projects.Remove(projectToBeRemoved);
                dbContext.SaveChanges();

                var projects = dbContext.Projects.Take(10);

                foreach (var project in projects)
                {
                    Console.WriteLine($"{project.Name}");
                }
            }
        }
    }
}
