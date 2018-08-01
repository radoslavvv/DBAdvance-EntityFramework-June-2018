using System;
using System.Collections.Generic;

namespace ProductsShop.Models
{
    public class User
    {
        public User()
        {
            this.ProductsBought = new List<Product>();
            this.ProductsSold = new List<Product>();
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

        public ICollection<Product> ProductsSold { get; set; }

        public ICollection<Product> ProductsBought { get; set; }
    }
}
