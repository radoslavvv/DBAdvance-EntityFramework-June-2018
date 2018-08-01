using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.App.Core.DTOs
{
    public class ManagerDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int ManagedEmployeesCount => this.ManagedEmployees.Count;

        public ICollection<EmployeeDTO> ManagedEmployees { get; set; }
    }
}
