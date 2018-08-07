using System.Xml.Serialization;

namespace ProductsShop.ExportData.DTOs
{
    [XmlType("product")]
    public class TwoProductDTO
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }
    }
}
