namespace PetClinic.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using PetClinic.Data;
    using PetClinic.DataProcessor.ExportDTOs;

    public class Serializer
    {
        public static string ExportAnimalsByOwnerPhoneNumber(PetClinicContext context, string phoneNumber)
        {
            var result = context.Animals.Where(a => a.Passport.OwnerPhoneNumber == phoneNumber).Select(a => new
            {
                OwnerName = a.Passport.OwnerName,
                AnimalName = a.Name,
                Age = a.Age,
                SerialNumber = a.PassportSerialNumber,
                RegisteredOn = a.Passport.RegistrationDate.ToString("dd-MM-yyyy")
            })
            .OrderBy(a => a.Age)
            .ThenBy(a => a.SerialNumber)
            .ToArray();

            string json = JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);

            return json;
        }

        public static string ExportAllProcedures(PetClinicContext context)
        {
            ProcedureDTO[] result = context.Procedures.OrderBy(p => p.DateTime)
                .Select(p => new ProcedureDTO()
                {
                    Passport = p.Animal.PassportSerialNumber,
                    OwnerNumber = p.Animal.Passport.OwnerPhoneNumber,
                    DateTime = p.DateTime.ToString("dd-MM-yyyy"),
                    AnimalAids = p.ProcedureAnimalAids.Select(pe => new AnimalAidDTO()
                    {
                        Name = pe.AnimalAid.Name,
                        Price = pe.AnimalAid.Price
                    }).ToArray(),
                    TotalPrice = p.ProcedureAnimalAids.Sum(pa => pa.AnimalAid.Price)
                }).OrderBy(p => p.Passport).ToArray();

            StringBuilder sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(ProcedureDTO[]), new XmlRootAttribute("Procedures"));

            serializer.Serialize(new StringWriter(sb), result, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));

            return sb.ToString().Trim();
        }
    }
}
