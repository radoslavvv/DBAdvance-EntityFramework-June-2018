using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductsShop.Data;
using ProductsShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace ProductsShop.ImportData
{
    public class StartUp
    {
        public static void Main()
        {
            ProductsShopDbContext context = new ProductsShopDbContext();

            context.Database.EnsureDeleted();
            context.Database.Migrate();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ProductsShopProfile>();
            });

            ExtractUsersData(context);

            ExtractProductsData(context);

            ExtractCategoriesData(context);

            ExtractCategoryProductsData(context);
        }

        private static void ExtractCategoryProductsData(ProductsShopDbContext context)
        {
            List<CategoryProduct> categoryProducts = new List<CategoryProduct>();

            Random randomizer = new Random();
            for (int i = 1; i < 200; i++)
            {
                CategoryProduct categoryProduct = new CategoryProduct()
                {
                    CategoryId = randomizer.Next(1, 11),
                    ProductId = i
                };

                categoryProducts.Add(categoryProduct);
            }

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();
        }

        private static void ExtractCategoriesData(ProductsShopDbContext context)
        {
            string jsonStr = File.ReadAllText("JSONs/categories.json");

            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(jsonStr);

            context.Categories.AddRange(categories);
            context.SaveChanges();
        }

        private static void ExtractProductsData(ProductsShopDbContext context)
        {
            string jsonStr = File.ReadAllText("JSONs/products.json");

            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(jsonStr);

            Random randomizer = new Random();
            foreach (Product product in products)
            {
                product.SellerId = randomizer.Next(1, 56);

                bool productHasBuyer = randomizer.Next(1, 5) == 5;
                if (productHasBuyer)
                {
                    product.BuyerId = randomizer.Next(1, 56);
                }
            }

            context.Products.AddRange(products);
            context.SaveChanges();
        }

        private static void ExtractUsersData(ProductsShopDbContext context)
        {
            string jsonStr = File.ReadAllText("JSONs/users.json");

            List<User> users = JsonConvert.DeserializeObject<List<User>>(jsonStr);

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
