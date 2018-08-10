using PetClinic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace PetClinic.DataProcessor.ImportDTOs
{
    [XmlType("Procedure")]
    public class ProcedureDTO
    {
        [Required]
        public string Vet { get; set; }


        [Required]
        public string Animal { get; set; }

        [Required]
        public string DateTime { get; set; }

        public AnimalAidProcedureDTO[] AnimalAids { get; set; }
    }
}
