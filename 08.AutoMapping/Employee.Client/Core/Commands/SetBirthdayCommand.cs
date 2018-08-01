using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Shop.Data;
using Shop.Models;

namespace Shop.App.Core.Commands
{
    public class SetBirthdayCommand : Command
    {
        public override string Execute(ShopDbContext context, List<string> arguments)
        {
            int employeeId = int.Parse(arguments[0]);

            DateTime date = DateTime.ParseExact(arguments[1], "dd-MM-yyyy", CultureInfo.InvariantCulture);

            Employee employee = context.Employees.FirstOrDefault(e => e.Id == employeeId);
            if (employee == null)
            {
                return $"There is no employee with id: {employeeId} in the database!";
            }

            employee.Birthdate= date;
            context.SaveChanges();

            return $"Employee {employee.FirstName} {employee.LastName}'s birthdate was updated successfully!";
        }
    }
}
