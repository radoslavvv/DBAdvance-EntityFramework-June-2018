using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductsShop.Data;
using ProductsShop.ExportData.DTOs;
using ProductsShop.ImportData;
using ProductsShop.Models;
using System;
using System.Collections.Generic;
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

            ExportSuccessullySoldProducts(context);

            ExportCategoriesByProductsCoutn(context);

            ExportUsersAndProducts(context);
        }

        private static void ExportUsersAndProducts(ProductsShopDbContext context)
        {
            var result = context.Users.Select(x => new
            {
                usersCount = context.Users.Count(),
                users = context
                    .Users
                    .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                    .OrderByDescending(u => u.ProductsSold.Count)
                    .ThenBy(u => u.LastName).Select(u => new
                    {
                        firstName = u.FirstName,
                        lastName = u.LastName,
                        age = u.Age,
                        soldProducts = new
                        {
                            count = u.ProductsSold.Count,
                            products = u.ProductsSold.Select(p => new
                            {
                                name = p.Name,
                                price = p.Price
                            })
                        }
                    })
            });


            string jsonStr = JsonConvert.SerializeObject(result, Formatting.Indented);
            File.WriteAllText("JSONs/users-and-products.json", jsonStr);
        }

        private static void ExportCategoriesByProductsCoutn(ProductsShopDbContext context)
        {
            var result = context
                .Categories
                .OrderBy(c => c.CategoryProducts.Count)
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoryProducts.Count,
                    averagePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    totalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                })
                .ToArray();


            string jsonStr = JsonConvert.SerializeObject(result, Formatting.Indented);
            File.WriteAllText("JSONs/categories-by-products.json", jsonStr);
        }

        private static void ExportSuccessullySoldProducts(ProductsShopDbContext context)
        {
            var result = context
                .Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                                        .Where(p => p.BuyerId != null)
                                        .Select(s => new
                                        {
                                            name = s.Name,
                                            price = s.Price,
                                            buyerFirstName = s.Buyer.FirstName,
                                            buyerLastName = s.Buyer.LastName
                                        }).ToArray()
                })
                .OrderBy(p => p.lastName)
                .ThenBy(p => p.firstName)
                .ToArray();


            string jsonStr = JsonConvert.SerializeObject(result, Formatting.Indented);
            File.WriteAllText("JSONs/users-sold-products.json", jsonStr);
        }

        private static void ExportProductsInRange(ProductsShopDbContext context)
        {
            var result = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = p.Seller.FirstName + " " + p.Seller.LastName
                })
                .ToList();


            string jsonStr = JsonConvert.SerializeObject(result, Formatting.Indented);
            File.WriteAllText("JSONs/products-in-range.json", jsonStr);
        }
    }
}
