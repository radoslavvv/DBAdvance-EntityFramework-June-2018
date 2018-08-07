using CarDealer.Data;
using CarDealer.ExportDataa.DTOs;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CarDealer.ExportDataa
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();

            ExportOrderedCustomers(context);

            ExportCarsFromToyota(context);

            ExportLocalSuppliers(context);

            ExportCarsWithListOfParts(context);

            ExportTotalSalesByCustomer(context);

            ExportSalesWithAppliedDiscount(context);
        }

        private static void ExportSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales.Select(s => new
            {
                car = new
                {
                    Make = s.Car.Make,
                    Model = s.Car.Model,
                    TravelledDistance = s.Car.TravelledDistance
                },
                customerName = s.Customer.Name,
                Discount = s.Discount,
                price = s.Car.PartCars.Sum(p => p.Part.Price),
                priceWithDiscount = s.Car.PartCars.Sum(p => p.Part.Price) - (s.Car.PartCars.Sum(p => p.Part.Price) * s.Discount / 100)
            });

            string jsonStr = JsonConvert.SerializeObject(sales, Formatting.Indented);
            File.WriteAllText("JSONs/sales-discounts.json", jsonStr);
        }

        private static void ExportTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers.Where(c => c.Sales.Count >= 1).Select(c => new
            {
                fullName = c.Name,
                boughtCars = c.Sales.Count,
                spentMoney = c.Sales.Sum(s => s.Car.PartCars.Sum(p => p.Part.Price)),

            });

            string jsonStr = JsonConvert.SerializeObject(customers, Formatting.Indented);
            File.WriteAllText("JSONs/customers=total-sales.json", jsonStr);
        }

        private static void ExportCarsWithListOfParts(CarDealerContext context)
        {
            var cars = context.Cars.Select(c => new
            {
                car = new
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                },
                parts = c.PartCars.Select(p => new
                {
                    Name = p.Part.Name,
                    Price = p.Part.Price
                })
            });

            string jsonStr = JsonConvert.SerializeObject(cars, Formatting.Indented);
            File.WriteAllText("JSONs/cars-and-parts.json", jsonStr);
        }

        private static void ExportLocalSuppliers(CarDealerContext context)
        {
            var localSuppliers = context.Suppliers.Where(s => !s.IsImporter).Select(s => new
            {
                Id = s.Id,
                Name = s.Name,
                PartsCount = s.Parts.Count
            });

            string jsonStr = JsonConvert.SerializeObject(localSuppliers, Formatting.Indented);
            File.WriteAllText("JSONs/local-suppliers.json", jsonStr);
        }

        private static void ExportCarsFromToyota(CarDealerContext context)
        {
            var cars = context
                .Cars
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                });

            string jsonStr = JsonConvert.SerializeObject(cars, Formatting.Indented);
            File.WriteAllText("JSONs/toyota-cars.json", jsonStr);
        }

        private static void ExportOrderedCustomers(CarDealerContext context)
        {
            var customers = context
                .Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver);

            string jsonStr = JsonConvert.SerializeObject(customers, Formatting.Indented);
            File.WriteAllText("JSONs/ordered-customers.json", jsonStr);
        }
    }
}
