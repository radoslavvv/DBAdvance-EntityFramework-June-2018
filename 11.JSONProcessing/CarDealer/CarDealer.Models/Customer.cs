using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CarDealer.Models
{
    public class Customer
    {
        public Customer()
        {
            this.Sales = new List<Sale>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public bool IsYoungDriver { get; set; }

        public  ICollection<Sale> Sales { get; set; }
    }
}
