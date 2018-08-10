using System.Xml.Serialization;

namespace PetClinic.DataProcessor.ImportDTOs
{
    [XmlType("AnimalAid")]
    public class AnimalAidProcedureDTO
    {
        public string Name { get; set; }
    }
}