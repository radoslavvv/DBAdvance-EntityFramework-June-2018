using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shop.App.Core;
using Shop.App.Core.Profiles;
using Shop.Data;
using System;

namespace Shop.App
{
    public class StartUp
    {
        public static void Main()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<EmployeesProfile>());

            using (ShopDbContext context = new ShopDbContext())
            {
                Engine engine = new Engine(context);
                engine.StartCommnadInterepreting();
            }
        }
    }
}
