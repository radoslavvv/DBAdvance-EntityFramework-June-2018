using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductsShop.ImportData
{
    [XmlType("category")]
    public class CategoryDTO
    {
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
