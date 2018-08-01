using System.Xml.Serialization;

namespace ProductsShop.ImportData
{
    [XmlType("user")]
    public class UserDTO
    {
        [XmlAttribute("firstName")]
        public string FirstName { get; set; }

        [XmlAttribute("lastName")]
        public string LastName { get; set; }

        [XmlAttribute("age")]
        public int Age { get; set; }
    }
}