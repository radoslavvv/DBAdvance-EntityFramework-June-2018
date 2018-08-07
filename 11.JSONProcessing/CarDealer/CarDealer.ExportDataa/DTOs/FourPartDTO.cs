using System.Xml.Serialization;

namespace CarDealer.ExportDataa.DTOs
{
    [XmlType("part")]
    public class FourPartDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public decimal Price { get; set; }
    }
}