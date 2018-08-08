using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Import;
using FastFood.Models;
using Newtonsoft.Json;

namespace FastFood.DataProcessor
{
    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportEmployees(FastFoodDbContext context, string jsonString)
        {
            EmployeeDTO[] deserializedEmployees = JsonConvert.DeserializeObject<EmployeeDTO[]>(jsonString);

            List<Employee> employees = new List<Employee>();
            StringBuilder sb = new StringBuilder();
            foreach (var employeeDTO in deserializedEmployees)
            {
                if (!IsValid(employeeDTO))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Position position = GetPosition(context, employeeDTO.Position);

                Employee employee = new Employee()
                {
                    Name = employeeDTO.Name,
                    Age = employeeDTO.Age,
                    Position = position
                };

                employees.Add(employee);

                sb.AppendLine(string.Format(SuccessMessage, employee.Name));
            }

            context.Employees.AddRange(employees);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        private static Position GetPosition(FastFoodDbContext context, string positionName)
        {
            Position currentPosition = context.Positions.FirstOrDefault(p => p.Name == positionName);

            if (currentPosition == null)
            {
                currentPosition = new Position()
                {
                    Name = positionName
                };

                context.Positions.Add(currentPosition);
                context.SaveChanges();
            }

            return currentPosition;
        }

        public static string ImportItems(FastFoodDbContext context, string jsonString)
        {
            ItemDTO[] deserializedItems = JsonConvert.DeserializeObject<ItemDTO[]>(jsonString);

            List<Item> items = new List<Item>();
            StringBuilder sb = new StringBuilder();
            foreach (var itemDTO in deserializedItems)
            {
                if (!IsValid(itemDTO))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                bool itemExists = items.Any(i => i.Name == itemDTO.Name);

                if (itemExists)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Category category = GetCategory(context, itemDTO.Category);

                Item item = new Item()
                {
                    Name = itemDTO.Name,
                    Price = itemDTO.Price,
                    Category = category
                };

                items.Add(item);
                sb.AppendLine(string.Format(SuccessMessage, item.Name));
            }

            context.Items.AddRange(items);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        private static Category GetCategory(FastFoodDbContext context, string categoryName)
        {
            Category currentCategory = context.Categories.FirstOrDefault(c => c.Name == categoryName);

            if (currentCategory == null)
            {
                currentCategory = new Category()
                {
                    Name = categoryName
                };

                context.Categories.Add(currentCategory);
                context.SaveChanges();
            }

            return currentCategory;
        }

        public static string ImportOrders(FastFoodDbContext context, string xmlString)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(OrderDTO[]), new XmlRootAttribute("Orders"));

            OrderDTO[] deserializedOrders = (OrderDTO[])xmlSerializer.Deserialize(new StringReader(xmlString));

            List<OrderItem> orderItems = new List<OrderItem>();
            List<Order> orders = new List<Order>();

            StringBuilder sb = new StringBuilder();
            foreach (var orderDTO in deserializedOrders)
            {
                bool isValidOrder = true;
                if (!IsValid(orderDTO))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                foreach (var itemDTO in orderDTO.OrderItemsDTO)
                {
                    if (!IsValid(itemDTO))
                    {
                        sb.AppendLine(FailureMessage);
                        isValidOrder = false;
                        break;
                    }
                }

                if (!isValidOrder)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Employee employee = context.Employees.FirstOrDefault(e => e.Name == orderDTO.Employeee);

                if (employee == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                bool itemsAreValid = ItemsAreValid(context, orderDTO.OrderItemsDTO);

                if (!itemsAreValid)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                DateTime date = DateTime.ParseExact(orderDTO.DateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                OrderType orderType = Enum.Parse<OrderType>(orderDTO.Type);

                Order order = new Order()
                {
                    Customer = orderDTO.Customer,
                    Employee = employee,
                    DateTime = date,
                    Type = orderType,
                };

                orders.Add(order);

                foreach (var itemDTO in orderDTO.OrderItemsDTO)
                {
                    Item item = context.Items.FirstOrDefault(i => i.Name == itemDTO.Name);

                    OrderItem orderItem = new OrderItem()
                    {
                        Order = order,
                        Item = item,
                        Quantity = itemDTO.Quantity
                    };

                    orderItems.Add(orderItem);
                }

                sb.AppendLine($"Order for {order.Customer} on {date.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)} added");
            }

            context.Orders.AddRange(orders);
            context.SaveChanges();

            context.OrderItems.AddRange(orderItems);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        private static bool ItemsAreValid(FastFoodDbContext context, OrderItemDTO[] orderItemsDTO)
        {
            foreach (var itemDTO in orderItemsDTO)
            {
                bool itemExists = context.Items.Any(i => i.Name == itemDTO.Name);

                if (!itemExists)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsValid(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);

            List<ValidationResult> validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}