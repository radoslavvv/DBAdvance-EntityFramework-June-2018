﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsShop.Models
{
    public class Category
    {
        public Category()
        {
            this.CategoryProducts = new List<CategoryProduct>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}
