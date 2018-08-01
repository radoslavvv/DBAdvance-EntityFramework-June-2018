using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shop.Data;
using Shop.Models;

namespace Shop.App.Core.Commands
{
    public class SetManagerCommand : Command
    {
        public override string Execute(ShopDbContext context, List<string> arguments)
        {
            int employeeId = int.Parse(arguments[0]);
            int managerId = int.Parse(arguments[1]);

            Employee employee = context.Employees.FirstOrDefault(e => e.Id == employeeId);
            if (employee == null)
            {
                return $"There is no employee with id: {employeeId} in the database!";
            }

            Employee manager = context.Employees.FirstOrDefault(e => e.ManagerId == managerId);
            if (employee == null)
            {
                return $"There is no manager with id: {managerId} in the database!";
            }

            employee.Manager = manager;
            context.SaveChanges();

            return $"{manager.FirstName} {manager.LastName} is manager of {employee.FirstName} {employee.LastName}!";
        }
    }
}
