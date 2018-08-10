namespace PetClinic.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Newtonsoft.Json;
    using PetClinic.Data;
    using PetClinic.DataProcessor.ImportDTOs;
    using PetClinic.Models;

    public class Deserializer
    {
        private const string ERROR_MESSAGE = "Error: Invalid data.";
        private const string SUCCESS_MESSAGE_ANIMAL_AIDS = "Record {0} successfully imported.";
        private const string SUCCESS_MESSAGE_ANIMALS = "Record {0} Passport №: {1} successfully imported.";

        private const string SUCCESS_MESSAGE_VETS = "Record {0} successfully imported.";

        private const string SUCCESS_MESSAGE_PROCEDURES = "Record successfully imported.";

        public static string ImportAnimalAids(PetClinicContext context, string jsonString)
        {
            AnimalAidDTO[] deserializedJson = JsonConvert.DeserializeObject<AnimalAidDTO[]>(jsonString);

            StringBuilder sb = new StringBuilder();
            List<AnimalAid> validAnimalAids = new List<AnimalAid>();

            foreach (var animalAidDTO in deserializedJson)
            {
                bool animalAidExists = validAnimalAids.Any(a => a.Name == animalAidDTO.Name);

                if (!IsValid(animalAidDTO) || animalAidExists)
                {
                    sb.AppendLine(ERROR_MESSAGE);
                    continue;
                }

                AnimalAid animalAid = Mapper.Map<AnimalAid>(animalAidDTO);
                validAnimalAids.Add(animalAid);
                sb.AppendLine(string.Format(SUCCESS_MESSAGE_ANIMAL_AIDS, animalAid.Name));
            }
            context.AnimalAids.AddRange(validAnimalAids);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportAnimals(PetClinicContext context, string jsonString)
        {
            AnimalDTO[] deserializedJson = JsonConvert.DeserializeObject<AnimalDTO[]>(jsonString);

            StringBuilder sb = new StringBuilder();
            List<Animal> validAnimals = new List<Animal>();

            foreach (var animalDTO in deserializedJson)
            {
                bool passportSerialNumberExists = validAnimals.Any(a => a.Passport.SerialNumber == animalDTO.Passport.SerialNumber);

                if (!IsValid(animalDTO) || !IsValid(animalDTO.Passport) || passportSerialNumberExists)
                {
                    sb.AppendLine(ERROR_MESSAGE);
                    continue;
                }

                Animal animal = Mapper.Map<Animal>(animalDTO);
                validAnimals.Add(animal);

                sb.AppendLine(string.Format(SUCCESS_MESSAGE_ANIMALS, animal.Name, animal.Passport.SerialNumber));
            }

            context.Animals.AddRange(validAnimals);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportVets(PetClinicContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(VetDTO[]), new XmlRootAttribute("Vets"));

            VetDTO[] deserializedXml = (VetDTO[])serializer.Deserialize(new StringReader(xmlString));

            List<Vet> validVets = new List<Vet>();
            StringBuilder sb = new StringBuilder();

            foreach (var vetDTO in deserializedXml)
            {
                bool vetExists = validVets.Any(v => v.PhoneNumber == vetDTO.PhoneNumber);

                if (!IsValid(vetDTO) || vetExists)
                {
                    sb.AppendLine(ERROR_MESSAGE);
                    continue;
                }

                Vet validVet = Mapper.Map<Vet>(vetDTO);
                validVets.Add(validVet);
                sb.AppendLine(string.Format(SUCCESS_MESSAGE_VETS, vetDTO.Name));
            }

            context.Vets.AddRange(validVets);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportProcedures(PetClinicContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ProcedureDTO[]), new XmlRootAttribute("Procedures"));

            ProcedureDTO[] deserializedXml = (ProcedureDTO[])serializer.Deserialize(new StringReader(xmlString));

            List<Procedure> validProcedures = new List<Procedure>();
            StringBuilder sb = new StringBuilder();

            foreach (var procedureDTO in deserializedXml)
            {
                Vet vet = context.Vets.FirstOrDefault(v => v.Name == procedureDTO.Vet);
                Animal animal = context.Animals.FirstOrDefault(a => a.PassportSerialNumber == procedureDTO.Animal);

                List<ProcedureAnimalAid> validProcedureAnimalAids = new List<ProcedureAnimalAid>();
                bool allAidsExists = true;
                foreach (var procedureDTOAnimalAid in procedureDTO.AnimalAids)
                {
                    AnimalAid animalAid = context.AnimalAids.FirstOrDefault(a => a.Name == procedureDTOAnimalAid.Name);
                    if (animalAid == null || validProcedureAnimalAids.Any(a => a.AnimalAid.Name == procedureDTOAnimalAid.Name))
                    {
                        allAidsExists = false;
                        break;
                    }

                    ProcedureAnimalAid procedureAnimalAid = new ProcedureAnimalAid()
                    {
                        AnimalAid = animalAid
                    };

                    validProcedureAnimalAids.Add(procedureAnimalAid);
                }

                if (!IsValid(procedureDTO) || !procedureDTO.AnimalAids.All(IsValid) || vet == null || animal == null || !allAidsExists)
                {
                    sb.AppendLine(ERROR_MESSAGE);
                    continue;
                }

                Procedure procedure = new Procedure()
                {
                    Animal = animal,
                    Vet = vet,
                    DateTime = DateTime.ParseExact(procedureDTO.DateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    ProcedureAnimalAids = validProcedureAnimalAids
                };

                validProcedures.Add(procedure);
                sb.AppendLine(SUCCESS_MESSAGE_PROCEDURES);
            }

            context.Procedures.AddRange(validProcedures);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        private static bool IsValid(object obj)
        {
            System.ComponentModel.DataAnnotations.ValidationContext context = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            List<ValidationResult> results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, context, results, true);

            return isValid;
        }
    }
}
