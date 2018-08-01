using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper.QueryableExtensions;
using Shop.App.Core.DTOs;
using Shop.Data;
using Shop.Models;

namespace Shop.App.Core.Commands
{
    public class ListEmployeesOlderThanCommand : Command
    {
        public override string Execute(ShopDbContext context, List<string> arguments)
        {
            int age = int.Parse(arguments[0]);

            EmployeeOlderThanDTO[] employees = context.Employees
                .Where(e => e.Birthdate.HasValue && e.Birthdate.Value.AddYears(age) <= DateTime.Now)
                .OrderByDescending(e => e.Salary)
                .ProjectTo<EmployeeOlderThanDTO>()
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                EmployeeDTO manager = employee.Manager;
                string managerStr = manager == null ? "(None)" : $"{manager.FirstName} {manager.LastName}";

                sb.AppendLine($"{employee.FirstName} {employee.LastName} - ${employee.Salary} - Manager: {managerStr}");
            }

            return sb.ToString().Trim();
        }
    }
}
