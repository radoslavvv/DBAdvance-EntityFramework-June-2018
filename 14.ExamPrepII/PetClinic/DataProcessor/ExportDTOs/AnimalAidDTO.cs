using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace PetClinic.DataProcessor.ExportDTOs
{
    [XmlType("AnimalAid")]
    public class AnimalAidDTO
    {
        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
