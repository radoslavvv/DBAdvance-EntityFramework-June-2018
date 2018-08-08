using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace FastFood.DataProcessor.Dto.Import
{
    [XmlType("Order")]
    public class OrderDTO
    {
        [Required]
        [XmlElement("Customer")]
        public string Customer { get; set; }

        [XmlElement("Employee")]
        public string Employeee { get; set; }

        [Required]
        [XmlElement("DateTime")]
        public string DateTime { get; set; }

        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlArray("Items")]
        public OrderItemDTO[] OrderItemsDTO { get; set; }
    }
}
