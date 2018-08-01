using System.Xml.Serialization;

namespace ProductsShop.ExportData.DTOs
{
    [XmlType("users")]
    public class FourUsersDTO
    {
        [XmlAttribute("count")]
        public int Count { get; set; }

        [XmlElement("user")]
        public FourUserDTO[] Users { get; set; }
    }
}
