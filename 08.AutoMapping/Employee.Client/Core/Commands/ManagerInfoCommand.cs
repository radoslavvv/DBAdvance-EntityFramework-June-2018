using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shop.App.Core.DTOs;
using Shop.Data;
using Shop.Models;

namespace Shop.App.Core.Commands
{
    public class ManagerInfoCommand : Command
    {
        public override string Execute(ShopDbContext context, List<string> arguments)
        {
            int managerId = int.Parse(arguments[0]);

            Employee manager = context.Employees
                .Include(e => e.ManagedEmployees)
                .FirstOrDefault(e => e.ManagerId == managerId);

            if (manager == null)
            {
                return $"There is no manager with Id:{managerId} in the database!";
            }

            ManagerDTO managerDto = Mapper.Map<ManagerDTO>(manager);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{managerDto.FirstName} {managerDto.LastName} | Employees: {managerDto.ManagedEmployeesCount}");

            foreach (var employee in managerDto.ManagedEmployees)
            {
                sb.AppendLine($"  - {employee.FirstName} {employee.LastName} - ${employee.Salary}");
            }

            return sb.ToString().Trim();
        }
    }
}
