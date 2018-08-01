using P02_DatabaseFirst.Data;
using System;
using System.Linq;

namespace _11.FindLatest10Projects
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (SoftUniContext dbContext = new SoftUniContext())
            {
                var projects = dbContext.Projects.OrderByDescending(p => p.StartDate).Take(10).Select(p => new
                {
                    Name = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate
                });

                foreach (var project in projects.OrderBy(p=>p.Name))
                {
                    Console.WriteLine($"{project.Name}");
                    Console.WriteLine($"{project.Description}");
                    Console.WriteLine($"{project.StartDate}");
                }
            }

        }
    }
}
