﻿using System.Xml.Serialization;

namespace ProductsShop.ExportData.DTOs
{
    [XmlType("user")]
    public class FourUserDTO
    {
        [XmlAttribute("first-name")]
        public string FirstName { get; set; }

        [XmlAttribute("last-name")]
        public string LastName { get; set; }

        [XmlAttribute("age")]
        public string Age { get; set; }

        [XmlElement("sold-products")]
        public FourProductsDTO SoldProducts { get; set; }
    }
}
