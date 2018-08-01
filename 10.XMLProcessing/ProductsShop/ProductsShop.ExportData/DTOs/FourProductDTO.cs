using System.Xml.Serialization;

namespace ProductsShop.ExportData.DTOs
{
    [XmlType("product")]
    public class FourProductDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public decimal Price { get; set; }
    }
}
