using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FastFood.DataProcessor.Dto.Export.XML
{
    [XmlType("Category")]
    public class CategoryDTO
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("MostPopularItem")]
        public MostPopularItemDTO MostPopularItem { get; set; }
    }
}
