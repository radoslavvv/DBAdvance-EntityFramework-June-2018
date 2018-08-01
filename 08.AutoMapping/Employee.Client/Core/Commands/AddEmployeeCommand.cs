using AutoMapper;
using Shop.App.Core.DTOs;
using Shop.Data;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.App.Core.Commands
{
    public class AddEmployeeCommand : Command
    {
        public override string Execute(ShopDbContext context, List<string> arguments)
        {
            EmployeeDTO employeeDto = new EmployeeDTO
            {
                FirstName = arguments[0],
                LastName = arguments[1],
                Salary = decimal.Parse(arguments[2])
            };

            Employee employee = Mapper.Map<Employee>(employeeDto);

            context.Employees.Add(employee);
            context.SaveChanges();

            return "The employee was added successfully!";
        }
    }
}
