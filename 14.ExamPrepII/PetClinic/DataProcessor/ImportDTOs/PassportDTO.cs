using System;
using System.ComponentModel.DataAnnotations;

namespace PetClinic.DataProcessor.ImportDTOs
{
    public class PassportDTO
    {
        [RegularExpression(@"^[A-Za-z]{7}[0-9]{3}$")]
        public string SerialNumber { get; set; }

        [Required]
        [MinLength(3), MaxLength(30)]
        public string OwnerName { get; set; }

        [Required]
        [RegularExpression(@"^(\+359\d{9})$|^(0\d{9})$")]
        public string OwnerPhoneNumber { get; set; }

        [Required]
        public string RegistrationDate { get; set; }
    }
}