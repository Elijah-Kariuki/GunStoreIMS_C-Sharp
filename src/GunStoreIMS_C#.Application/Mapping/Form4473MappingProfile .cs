using AutoMapper;
using GunStoreIMS.Domain.Models;
using GunStoreIMS.Application.Dto;


namespace GunStoreIMS.Application.Mapping
{
    public class Form4473MappingProfile : Profile
    {
        public Form4473MappingProfile()
        {
            CreateMap<Form4473Record, Form4473RecordDto>()
                .ReverseMap()
                // avoid cycles when you later add other child navigations:
                .PreserveReferences();

            CreateMap<Form4473FirearmLine, Form4473FirearmLineDto>()
                .ReverseMap();

            // Enum → string (and back, if you need the reverse)
            CreateMap<NicsResponseType, string>()
                .ConvertUsing(src => src.ToString());
        }
    }
}
