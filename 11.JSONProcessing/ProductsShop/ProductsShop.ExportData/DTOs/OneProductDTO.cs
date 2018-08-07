using System.Xml.Serialization;

namespace ProductsShop.ExportData.DTOs
{
    [XmlType("product")]
    public class OneProductDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public decimal Price { get; set; }

        [XmlAttribute("buyer")]
        public string Buyer { get; set; }
    }
}
