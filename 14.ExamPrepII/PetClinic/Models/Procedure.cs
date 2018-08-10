using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace PetClinic.Models
{
    public class Procedure
    {
        //-	Id – integer, Primary Key
        //-	AnimalId ¬– integer, foreign key
        //-	Animal – the animal on which the procedure is performed(required)
        //-	VetId ¬– integer, foreign key
        //-	Vet – the clinic’s employed doctor servicing the patient(required)
        //-	ProcedureAnimalAids – collection of type ProcedureAnimalAid
        //-	Cost – the cost of the procedure, calculated by summing the price of the different services performed; does not need to be inserted in the database
        //-	DateTime – the date and time on which the given procedure is performed(required)

        public Procedure()
        {
            this.ProcedureAnimalAids = new List<ProcedureAnimalAid>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Animal))]
        public int AnimalId { get; set; }
        public Animal Animal { get; set; }

        [Required]
        [ForeignKey(nameof(Vet))]
        public int VetId { get; set; }
        public Vet Vet { get; set; }

        public ICollection<ProcedureAnimalAid> ProcedureAnimalAids { get; set; }

        public decimal Cost => this.ProcedureAnimalAids.Sum(p => p.AnimalAid.Price);

        public DateTime DateTime { get; set; }
    }
}
