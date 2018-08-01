using AutoMapper;
using Shop.App.Core.DTOs;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.App.Core.Profiles
{
    public class EmployeesProfile : Profile
    {
        public EmployeesProfile()
        {
            CreateMap<Employee, EmployeeDTO>();
            CreateMap<EmployeeDTO, Employee>();
            CreateMap<Employee, ManagerDTO>();
            CreateMap<Employee, EmployeeOlderThanDTO>();
        }
    }
}
