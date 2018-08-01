using CarDealer.Data;
using CarDealer.ExportDataa.DTOs;
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

            ExportCarsWithDistance(context);
            ExportCarsFromFerrari(context);
            ExportLocalSuppliers(context);
            ExportCarParts(context);
            ExportTotalCustomerSales(context);
            ExportSalesWithAppliedDiscount(context);
        }

        public static void ExportCarsWithDistance(CarDealerContext context)
        {
            OneCarDTO[] cars = context
                .Cars
                .Where(c => c.TravelledDistance > 2_000_000)
                .Select(c => new OneCarDTO()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(OneCarDTO[]), new XmlRootAttribute("cars"));

            using (StreamWriter writer = new StreamWriter("cars-with-distance.xml"))
            {
                serializer.Serialize(writer, cars);
            }
        }

        public static void ExportCarsFromFerrari(CarDealerContext context)
        {
            TwoCarDTO[] carsFromFerrari = context.Cars.Where(c => c.Make == "Ferrari").Select(c => new TwoCarDTO()
            {
                Id = c.Id,
                Model = c.Model,
                TravelledDistance = c.TravelledDistance
            })
            .OrderBy(c => c.Model)
            .ThenByDescending(c => c.TravelledDistance)
            .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(TwoCarDTO[]), new XmlRootAttribute("cars"));

            using (StreamWriter writer = new StreamWriter("cars-from-ferrari.xml"))
            {
                serializer.Serialize(writer, carsFromFerrari);
            }
        }

        public static void ExportLocalSuppliers(CarDealerContext context)
        {
            ThreeSupplierDTO[] localSuppliers = context.Suppliers
                                  .Where(x => !x.IsImporter)
                                  .Select(x => new ThreeSupplierDTO
                                  {
                                      Id = x.Id,
                                      Name = x.Name,
                                      Count = x.Parts.Count
                                  })
                                  .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ThreeSupplierDTO[]), new XmlRootAttribute("suppliers"));
            using (StreamWriter writer = new StreamWriter("local-suppliers.xml"))
            {
                serializer.Serialize(writer, localSuppliers);
            }
        }

        public static void ExportCarParts(CarDealerContext context)
        {
            FourCarDTO[] cars = context.Cars
                            .Select(x => new FourCarDTO()
                            {
                                Make = x.Make,
                                Model = x.Model,
                                TravelledDistance = x.TravelledDistance,
                                Parts = x.PartCars
                                            .Select(p => new FourPartDTO()
                                            {
                                                Name = p.Part.Name,
                                                Price = p.Part.Price
                                            }).ToArray()
                            }).ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(FourCarDTO[]), new XmlRootAttribute("cars"));

            using (StreamWriter writer = new StreamWriter("cars-parts-list.xml"))
            {
                serializer.Serialize(writer, cars);
            }
        }

        public static void ExportTotalCustomerSales(CarDealerContext context)
        {
            FiveCustomerDTO[] customers = context
                                            .Customers
                                            .Where(x => x.Sales.Count >= 1)
                                           .Select(x => new FiveCustomerDTO()
                                           {
                                               FullName = x.Name,
                                               BoughtCars = x.Sales.Count,
                                               SpentMoney = x.Sales
                                                                .Sum(p => p.Car.PartCars.Sum(a => a.Part.Price))
                                           }).ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(FiveCustomerDTO[]), new XmlRootAttribute("customers"));

            using (var writer = new StreamWriter("customer-sales.xml"))
            {
                serializer.Serialize(writer, customers);
            }
        }

        public static void ExportSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                  .Select(x => new SixSaleDTO()
                  {
                      CustomerName = x.Customer.Name,
                      Price = x.Car.PartCars.Sum(p => p.Part.Price),
                      Discount = x.Discount,
                      TotalPrice = x.Car.PartCars.Sum(p => p.Part.Price) - (x.Car.PartCars.Sum(p => p.Part.Price) * x.Discount / 100.0m),
                      Car = new SixCarDTO()
                      {
                          Make = x.Car.Make,
                          Model = x.Car.Model,
                          TravelledDistance = x.Car.TravelledDistance
                      }
                  }).ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(SixSaleDTO[]), new XmlRootAttribute("sales"));

            using (StreamWriter writer = new StreamWriter("sales-discounts.xml"))
            {
                serializer.Serialize(writer, sales);
            }
        }

    }
}
