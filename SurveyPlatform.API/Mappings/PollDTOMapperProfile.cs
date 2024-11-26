using AutoMapper;
using SurveyPlatform.API.DTOs.Requests;
using SurveyPlatform.BLL.Models;
using SurveyPlatform.DAL.Entities;
using SurveyPlatform.DTOs.Responses;

namespace SurveyPlatform.API.Mappings
{
    public class PollDTOMapperProfile:Profile
    {
        public PollDTOMapperProfile()
        {
            CreateMap<CreatePollRequest, PollModel>();
            CreateMap<PollResponse, PollResultsResponse>().ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Poll.Options.Select(o => new OptionResult
            {
                OptionId = o.Id,
                Content = o.Content,
                
            }).ToList()));
            CreateMap<PollModel, PollDataResponse>();
            CreateMap<PollOptionModel, OptionResponse>(); //варианты для голоса
            CreateMap<PollResponseModel, PollSubmittedResponse>(); //голоса людей
            CreateMap<UpdatePollRequest, UpdatePollModel>(); //обновление опроса
        }
    }
}
