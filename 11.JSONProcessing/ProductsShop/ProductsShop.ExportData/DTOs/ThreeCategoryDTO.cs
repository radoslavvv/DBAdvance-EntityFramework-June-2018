using System.Xml.Serialization;

namespace ProductsShop.ExportData.DTOs
{
    [XmlType("category")]
    public class ThreeCategoryDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("products-count")]
        public int Count { get; set; }

        [XmlElement("average-price")]
        public decimal AveragePrice { get; set; }

        [XmlElement("total-revenue")]
        public decimal TotalRevenue { get; set; }
    }
}
