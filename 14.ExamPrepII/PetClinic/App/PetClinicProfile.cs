namespace PetClinic.App
{
    using AutoMapper;
    using PetClinic.DataProcessor.ImportDTOs;
    using PetClinic.Models;
    using System;
    using System.Globalization;

    public class PetClinicProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public PetClinicProfile()
        {
            CreateMap<AnimalAidDTO, AnimalAid>();
            CreateMap<AnimalDTO, Animal>();
            CreateMap<PassportDTO, Passport>()
                .ForMember(p => p.RegistrationDate, rd => rd.MapFrom(dto => DateTime.ParseExact(dto.RegistrationDate, "dd-MM-yyyy", CultureInfo.InvariantCulture)));

            CreateMap<VetDTO, Vet>();
        }
    }
}
