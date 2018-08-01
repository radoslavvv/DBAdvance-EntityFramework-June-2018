using ProductsShop.Data;
using ProductsShop.ExportData.DTOs;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace ProductsShop.ExportData
{
    public class StartUp
    {
        public static void Main()
        {
            ProductsShopDbContext context = new ProductsShopDbContext();

            ExportProductsInRange(context);

            ExportSoldProducts(context);

            ExportCategoriesByProductsCount(context);

            ExportUsersWithProducts(context);
        }

        public static void ExportProductsInRange(ProductsShopDbContext context)
        {
            OneProductDTO[] products = context.Products
                                  .Where(x => x.Price >= 1000 && x.Price <= 2000 && x.Buyer != null)
                                  .Select(x => new OneProductDTO()
                                  {
                                      Name = x.Name,
                                      Price = x.Price,
                                      Buyer = x.Buyer.FirstName + " " + x.Buyer.LastName ?? x.Buyer.LastName
                                  })
                                  .OrderBy(x => x.Price)
                                  .ToArray();



            XmlSerializer serializer = new XmlSerializer(typeof(OneProductDTO[]), new XmlRootAttribute("products"));

            using (var writer = new StreamWriter("products-in-range.xml"))
            {
                serializer.Serialize(writer, products);
            }
        }

        public static void ExportSoldProducts(ProductsShopDbContext context)
        {
            TwoUserDTO[] users = context.Users
                              .Where(x => x.ProductsBought.Count >= 1)
                              .Select(x => new TwoUserDTO
                              {
                                  FirstName = x.FirstName,
                                  LastName = x.LastName,
                                  SoldProducts = x.ProductsBought.Select(p => new TwoProductDTO
                                  {
                                      Name = p.Name,
                                      Price = p.Price
                                  }).ToArray()
                              })
                              .OrderBy(x => x.LastName)
                              .ThenBy(x => x.FirstName)
                              .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(TwoUserDTO[]), new XmlRootAttribute("users"));

            using (var writer = new StreamWriter("users-sold-products.xml"))
            {
                serializer.Serialize(writer, users);
            }
        }

        public static void ExportCategoriesByProductsCount(ProductsShopDbContext context)
        {
            ThreeCategoryDTO[] categories = context.Categories
                                    .Select(x => new ThreeCategoryDTO()
                                    {
                                        Name = x.Name,
                                        Count = x.CategoryProducts.Count,
                                        AveragePrice = x.CategoryProducts.Average(a => a.Product.Price),
                                        TotalRevenue = x.CategoryProducts.Sum(s => s.Product.Price)
                                    })
                                    .OrderByDescending(x => x.Count)
                                    .ToArray();


            XmlSerializer serializer = new XmlSerializer(typeof(ThreeCategoryDTO[]), new XmlRootAttribute("categories"));

            using (var writer = new StreamWriter("categories-by-products.xml"))
            {
                serializer.Serialize(writer, categories);
            }
        }

        public static void ExportUsersWithProducts(ProductsShopDbContext context)
        {
            FourUserDTO[] users = context
                .Users
                .Where(x => x.ProductsSold.Any(a => a.BuyerId != null))
                               .Select(x => new FourUserDTO
                               {
                                   FirstName = x.FirstName,
                                   LastName = x.LastName,
                                   Age = x.Age.ToString(),
                                   SoldProducts = new FourProductsDTO
                                   {
                                       Count = x.ProductsSold.Where(b => b.BuyerId != null).Count(),
                                       Product = x.ProductsSold.Where(b => b.BuyerId != null)
                                                    .Select(a => new FourProductDTO()
                                                    {
                                                        Name = a.Name,
                                                        Price = a.Price
                                                    }).ToArray()
                                   }
                               })
                               .OrderByDescending(x => x.SoldProducts.Count)
                               .ToArray();

            FourUsersDTO usersProducts = new FourUsersDTO
            {
                Count = users.Count(),
                Users = users
            };

            XmlSerializer serializer = new XmlSerializer(typeof(FourUsersDTO));

            using (StreamWriter writer = new StreamWriter("users-and-products.xml"))
            {
                serializer.Serialize(writer, usersProducts);
            }
        }
    }
}
