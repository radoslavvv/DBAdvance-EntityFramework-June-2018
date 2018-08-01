using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.ExportDataa.DTOs
{
    [XmlType("sale")]
    public class SixSaleDTO
    {
        [XmlElement("customer-name")]
        public string CustomerName { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("discount")]
        public decimal Discount { get; set; }

        [XmlElement("price-with-discount")]
        public decimal TotalPrice { get; set; }

        [XmlElement("car")]
        public SixCarDTO Car { get; set; }
    }
}
