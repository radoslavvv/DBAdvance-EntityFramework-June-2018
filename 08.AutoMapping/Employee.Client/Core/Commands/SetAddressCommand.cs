using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shop.Data;
using Shop.Models;

namespace Shop.App.Core.Commands
{
    public class SetAddressCommand : Command
    {
        public override string Execute(ShopDbContext context, List<string> arguments)
        {
            int employeeId = int.Parse(arguments[0]);
            string employeeAddress = string.Join(' ', arguments.Skip(1));

            Employee employee = context.Employees
                .FirstOrDefault(e => e.Id == employeeId);

            if (employee == null)
            {
                return $"There is no employee with id: {employeeId} in the database!";
            }

            employee.Address = employeeAddress;
            context.SaveChanges();

            return $"Employee {employee.FirstName} {employee.LastName}'s address was updated successfully!";
        }
    }
}
