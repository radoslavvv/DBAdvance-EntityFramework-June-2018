using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetClinic.DataProcessor.ImportDTOs
{
   public  class AnimalDTO
    {
        [Required]
        [MinLength(3), MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [MinLength(3), MaxLength(20)]
        public string Type { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Age { get; set; }

        public PassportDTO Passport { get; set; }
    }
}
