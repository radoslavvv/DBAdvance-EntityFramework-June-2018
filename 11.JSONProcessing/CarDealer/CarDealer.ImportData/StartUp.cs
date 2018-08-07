using AutoMapper;
using CarDealer.Data;
using CarDealer.ImportData.DTOs;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CarDealer.ImportData
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();

            context.Database.EnsureDeleted();
            context.Database.Migrate();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Supplier, SupplierDTO>().ReverseMap();
                cfg.CreateMap<Part, PartDTO>().ReverseMap();
                cfg.CreateMap<Car, CarDTO>().ReverseMap();
                cfg.CreateMap<Customer, CustomerDTO>().ReverseMap();
            });

            ExtractSuppliersData(context);

            ExtractPartsData(context);

            ExtractCarsData(context);

            ExtractPartCarsData(context);

            ExtractCustomersData(context);

            ExtractSalesData(context);
        }

        private static void ExtractSalesData(CarDealerContext context)
        {
            List<Sale> sales = new List<Sale>();
            int[] discounts = new int[] { 0, 5, 10, 15, 20, 30, 40, 50 };

            List<int> carIds = new List<int>();
            List<int> customerIds = new List<int>();

            Random randomizer = new Random();

            for (int i = 0; i < 100; i++)
            {
                int discountIndex = randomizer.Next(0, 8);
                int carId = randomizer.Next(1, 359);
                int customerId = randomizer.Next(1, 31);

                if (carIds.Contains(carId) && customerIds.Contains(customerId))
                {
                    continue;
                }

                customerIds.Add(customerId);
                carIds.Add(carId);

                Sale sale = new Sale
                {
                    CarId = carId,
                    CustomerId = customerId,
                    Discount = discounts[discountIndex]
                };

                sales.Add(sale);
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();
        }

        private static void ExtractCustomersData(CarDealerContext context)
        {
            string jsonStr = File.ReadAllText("JSONs/customers.json");

            List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(jsonStr);

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }

        private static void ExtractPartCarsData(CarDealerContext context)
        {
            List<PartCar> partCars = new List<PartCar>();
            Random randomizer = new Random();

            for (int i = 1; i <= 358; i++)
            {
                int randomNumberOfParts = randomizer.Next(10, 21);

                List<int> partIds = new List<int>();
                for (int j = 0; j < randomNumberOfParts; j++)
                {
                    int randomPartId = randomizer.Next(1, 132);
                    if (partIds.Contains(randomPartId))
                    {
                        continue;
                    }

                    partIds.Add(randomPartId);

                    PartCar currentPartCar = new PartCar
                    {
                        CarId = i,
                        PartId = randomPartId
                    };

                    partCars.Add(currentPartCar);
                }
            }
            context.PartCars.AddRange(partCars);
            context.SaveChanges();
        }

        private static void ExtractCarsData(CarDealerContext context)
        {
            string jsonStr = File.ReadAllText("JSONs/cars.json");

            List<Car> cars = JsonConvert.DeserializeObject<List<Car>>(jsonStr);

            context.Cars.AddRange(cars);
            context.SaveChanges();
        }

        private static void ExtractPartsData(CarDealerContext context)
        {
            string jsonStr = File.ReadAllText("JSONs/parts.json");

            List<Part> parts = JsonConvert.DeserializeObject<List<Part>>(jsonStr);

            foreach (var part in parts)
            {
                part.SupplerId = new Random().Next(1, context.Suppliers.Count() - 1);

            }

            context.Parts.AddRange(parts);
            context.SaveChanges();
        }

        private static void ExtractSuppliersData(CarDealerContext context)
        {
            string jsonStr = File.ReadAllText("JSONs/suppliers.json");

            List<Supplier> suppliers = JsonConvert.DeserializeObject<List<Supplier>>(jsonStr);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();
        }
    }
}
