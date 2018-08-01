using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
            ExtractCategoryProducts(context);
        }

        private static void ExtractCategoryProducts(ProductsShopDbContext context)
        {
            List<CategoryProduct> categoryProducts = new List<CategoryProduct>();

            Random randomizer = new Random();
            for (int i = 1; i <= 200; i++)
            {
                CategoryProduct categoryProduct = new CategoryProduct
                {
                    CategoryId = randomizer.Next(1, 12),
                    ProductId = i
                };

                categoryProducts.Add(categoryProduct);
            }

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();
        }

        private static void ExtractProductsData(ProductsShopDbContext context)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ProductDTO[]), new XmlRootAttribute("products"));
            string xmlProducts = File.ReadAllText(@"XMLs\products.xml");

            ProductDTO[] productsDto = (ProductDTO[])serializer.Deserialize(new StringReader(xmlProducts));

            Product[] products = productsDto
                .Where(x => IsValid(x))
                .Select(x => Mapper.Map<Product>(x))
                .ToArray();

            Random randomizer = new Random();
            foreach (Product product in products)
            {
                product.SellerId = randomizer.Next(1, 57);

                bool productHasNoBuyer = randomizer.Next(1, 5) == 1;
                if (productHasNoBuyer)
                {
                    continue;
                }

                product.BuyerId = randomizer.Next(1, 57);
            }

            context.Products.AddRange(products);
            context.SaveChanges();
        }

        private static void ExtractUsersData(ProductsShopDbContext context)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserDTO[]), new XmlRootAttribute("users"));
            string xmlUsers = File.ReadAllText(@"XMLs\users.xml");

            UserDTO[] usersDto = (UserDTO[])serializer.Deserialize(new StringReader(xmlUsers));

            User[] users = usersDto
                .Where(x => IsValid(x)).Select(x => Mapper.Map<User>(x))
                .ToArray();

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        private static void ExtractCategoriesData(ProductsShopDbContext context)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CategoryDTO[]), new XmlRootAttribute("categories"));
            string xmlCategories = File.ReadAllText(@"XMLs\categories.xml");

            CategoryDTO[] categoriesDto = (CategoryDTO[])serializer.Deserialize(new StringReader(xmlCategories));

            Category[] categories = categoriesDto
                .Where(x => IsValid(x))
                .Select(x => Mapper.Map<Category>(x))
                .ToArray();

            context.Categories.AddRange(categories);
            context.SaveChanges();
        }

        private static bool IsValid(object obj)
        {
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}
