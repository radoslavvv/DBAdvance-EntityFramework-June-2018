using System.Xml.Serialization;

namespace ProductsShop.ExportData.DTOs
{
    [XmlType("user")]
    public class TwoUserDTO
    {
        [XmlAttribute("first-name")]
        public string FirstName { get; set; }

        [XmlAttribute("last-name")]
        public string LastName { get; set; }

        [XmlArray("sold-products")]
        public TwoProductDTO[] SoldProducts { get; set; }
    }
}
