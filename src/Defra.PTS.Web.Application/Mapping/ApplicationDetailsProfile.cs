using AutoMapper;
using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Mapping.Converters;

namespace Defra.PTS.Web.Application.Mapping;

public class ApplicationDetailsProfile : Profile
{
    public ApplicationDetailsProfile()
    {
        CreateMap<VwApplication, ApplicationDetailsDto>()
            .ForMember(dest => dest.Status, opt => opt.ConvertUsing(new ConvertDisplayStatus(), src => src.Status))
            .ForMember(dest => dest.MicrochipInformation, opt => opt.MapFrom(src => MappingConverter.MapMicrochipInformation(src)))
            .ForMember(dest => dest.PetDetails, opt => opt.MapFrom(src => MappingConverter.MapPetDetails(src)))
            .ForMember(dest => dest.PetKeeperDetails, opt => opt.MapFrom(src => MappingConverter.MapPetKeeperDetails(src)))
            .ForMember(dest => dest.Declaration, opt => opt.MapFrom(src => MappingConverter.MapDeclaration(src)));
    }
}
