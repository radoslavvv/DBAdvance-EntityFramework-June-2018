using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace PetClinic.DataProcessor.ExportDTOs
{
    [XmlType("Procedure")]
   public class ProcedureDTO
    {
        public string Passport { get; set; }

        public string OwnerNumber { get; set; }

        public string DateTime { get; set; }

        public AnimalAidDTO[] AnimalAids { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
