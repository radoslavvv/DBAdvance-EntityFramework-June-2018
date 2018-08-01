using AutoMapper;
using CarDealer.Data;
using CarDealer.ImportData.DTOs;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
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

        private static void ExtractSuppliersData(CarDealerContext context)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SupplierDTO[]), new XmlRootAttribute("suppliers"));

            string xmlSuppliersString = File.ReadAllText("XMLs\\suppliers.xml");
            SupplierDTO[] suppliersDTO = (SupplierDTO[])serializer.Deserialize(new StringReader(xmlSuppliersString));

            Supplier[] suppliers = suppliersDTO.Select(s => Mapper.Map<Supplier>(s)).ToArray();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();
        }

        private static void ExtractPartsData(CarDealerContext context)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PartDTO[]), new XmlRootAttribute("parts"));

            string xmlPartsString = File.ReadAllText("XMLs\\parts.xml");
            PartDTO[] partsDTO = (PartDTO[])serializer.Deserialize(new StringReader(xmlPartsString));

            Random randomizer = new Random();
            List<Part> parts = new List<Part>();
            for (int i = 0; i < partsDTO.Length; i++)
            {
                Part currentPart = Mapper.Map<Part>(partsDTO[i]);

                currentPart.SupplerId = randomizer.Next(1, 32);

                parts.Add(currentPart);
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();
        }

        private static void ExtractCustomersData(CarDealerContext context)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CustomerDTO[]), new XmlRootAttribute("customers"));

            string xmlCustomersString = File.ReadAllText("XMLs\\customers.xml");
            CustomerDTO[] customersDTO = (CustomerDTO[])serializer.Deserialize(new StringReader(xmlCustomersString));


            Customer[] customers = customersDTO.Select(s => Mapper.Map<Customer>(s)).ToArray();

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }

        private static void ExtractCarsData(CarDealerContext context)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CarDTO[]), new XmlRootAttribute("cars"));

            string xmlCarsString = File.ReadAllText("XMLs\\cars.xml");
            CarDTO[] carsDTO = (CarDTO[])serializer.Deserialize(new StringReader(xmlCarsString));

            List<Car> cars = new List<Car>();
            for (int i = 0; i < carsDTO.Length; i++)
            {
                Car currentCar = Mapper.Map<Car>(carsDTO[i]);

                cars.Add(currentCar);
            }

            context.Cars.AddRange(cars);
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
    }
}
