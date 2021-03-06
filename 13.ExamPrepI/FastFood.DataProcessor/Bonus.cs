﻿using System;
using System.Linq;
using FastFood.Data;
using FastFood.Models;

namespace FastFood.DataProcessor
{
    public static class Bonus
    {
        public static string UpdatePrice(FastFoodDbContext context, string itemName, decimal newPrice)
        {
            Item item = context.Items.FirstOrDefault(i => i.Name == itemName);
            if (item == null)
            {
                return $"Item {itemName} not found!";
            }
            else
            {
                decimal itemOldPrice = item.Price;
                item.Price = newPrice;

                context.SaveChanges();

                return $"{item.Name} Price updated from ${itemOldPrice:F2} to ${newPrice:F2}";
            }
        }
    }
}
