using AutoMapper;
using Shop.App.Core.DTOs;
using Shop.Data;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.App.Core.Commands
{
    public class EmployeeInfoCommand:Command
    {
        public override string Execute(ShopDbContext context, List<string> arguments)
        {
            int employeeId = int.Parse(arguments[0]);

            Employee employee = context.Employees.FirstOrDefault(e=>e.Id == employeeId);
            if (employee == null)
            {
                return $"There is no employee with id: {employeeId} in the database!";
            }

            EmployeeDTO employeeDto = Mapper.Map<EmployeeDTO>(employee);

            return employee.ToString();
        }
    }
}
