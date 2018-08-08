using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Export.XML;
using FastFood.Models;
using Newtonsoft.Json;

namespace FastFood.DataProcessor
{
    public class Serializer
    {
        public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
        {
            OrderType orderTypeParsed = Enum.Parse<OrderType>(orderType);

            var result = context
                .Employees
                .ToArray() // remove
                .Where(e => e.Name == employeeName)
                .Select(x => new
                {
                    Name = x.Name,
                    Orders = x.Orders
                    .Where(o => o.Type == orderTypeParsed)
                    .Select(o => new
                    {
                        Customer = o.Customer,
                        Items = o.OrderItems
                            .Select(oi => new
                            {
                                Name = oi.Item.Name,
                                Price = oi.Item.Price,
                                Quantity = oi.Quantity
                            }).ToArray(),
                        TotalPrice = o.TotalPrice
                    }).ToArray()
                    .OrderByDescending(c => c.TotalPrice)
                    .ThenByDescending(c => c.Items.Length),
                    TotalMade = x.Orders
                        .Where(o => o.Type == orderTypeParsed)
                        .Sum(o => o.TotalPrice)
                }).FirstOrDefault();

            string jsonStr = JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
            return jsonStr;
        }

        public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
        {
            string[] categories = categoriesString.Split(',');
            var result = context.Categories.Where(s => categories.Contains(s.Name)).Select(x => new
            {
                Name = x.Name,
                MostPopularItem = x.Items
                    .Select(s => new MostPopularItemDTO()
                    {
                        Name = s.Name,
                        TimesSold = s.OrderItems.Sum(t => t.Quantity),
                        TotalMade = s.OrderItems.Sum(t => t.Item.Price * t.Quantity)
                    })
                    .OrderByDescending(i => i.TotalMade)
                    .ThenByDescending(i => i.TimesSold)
                    .FirstOrDefault()
            }).OrderByDescending(x => x.MostPopularItem.TotalMade)
            .ThenByDescending(x => x.MostPopularItem.TimesSold)
            .ToArray();

            StringBuilder sb = new StringBuilder();

            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            XmlSerializer serializer = new XmlSerializer(typeof(CategoryDTO[]), new XmlRootAttribute("Categories"));
            serializer.Serialize(new StringWriter(sb), categories, xmlNamespaces);

            return sb.ToString().Trim();
        }
    }
}