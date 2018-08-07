using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace ProductsShop.ImportData
{
    [XmlType("product")]
    public class ProductDTO
    {
        [XmlElement("name")]
        [MinLength(3)]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("buyer")]
        public UserDTO Buyer { get; set; }

        [XmlElement("seller")]
        public UserDTO Seller { get; set; }
    }
}
