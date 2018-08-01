using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.App.Core.DTOs
{
    public class EmployeeOlderThanDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public EmployeeDTO Manager { get; set; }
    }
}
