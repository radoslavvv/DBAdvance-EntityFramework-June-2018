using System.Xml.Serialization;

namespace ProductsShop.ExportData.DTOs
{
    [XmlType("sold-products")]
    public class FourProductsDTO
    {
        [XmlAttribute("count")]
        public int Count { get; set; }

        [XmlElement("product")]
        public FourProductDTO[] Product { get; set; }
    }
}
