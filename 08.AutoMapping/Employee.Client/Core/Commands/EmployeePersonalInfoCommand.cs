using Shop.Data;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Shop.App.Core.Commands
{
    public class EmployeePersonalInfoCommand:Command
    {
        public override string Execute(ShopDbContext context, List<string> arguments)
        {
            int employeeId = int.Parse(arguments[0]);

            Employee employee = context.Employees.FirstOrDefault(e => e.Id == employeeId);
            if (employee == null)
            {
                return $"There is no employee with id: {employeeId} in the database!";
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(employee.ToString());

            string employeeBirthdate = employee.Birthdate?.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) ?? "(None)";
            sb.AppendLine($"Birthday: {employeeBirthdate}");

            string employeeAddress = employee.Address ?? "N/A";
            sb.AppendLine($"Address: {employeeAddress}");

            return sb.ToString().Trim();
        }
    }
}
