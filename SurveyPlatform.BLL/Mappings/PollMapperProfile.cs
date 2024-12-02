using AutoMapper;
using SurveyPlatform.BLL.Models;
using SurveyPlatform.DAL.Entities;

namespace SurveyPlatform.BLL.Mappings;
public class PollMapperProfile : Profile
{
    public PollMapperProfile()
    {
        CreateMap<PollModel, Poll>();
        CreateMap<SubmitResponseModel, PollResponse>();
        CreateMap<Poll, PollModel > ();
        CreateMap<string, PollOptionModel>()
        .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src));
        CreateMap<PollOptionModel, PollOption>();
        CreateMap<PollOption, PollOptionModel>();
        CreateMap<PollResponse, PollResponseModel>();
    }
}
