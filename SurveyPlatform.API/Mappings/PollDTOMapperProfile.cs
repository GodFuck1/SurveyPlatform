using AutoMapper;
using SurveyPlatform.API.DTOs.Requests;
using SurveyPlatform.BLL.Models;

namespace SurveyPlatform.API.Mappings
{
    public class PollDTOMapperProfile:Profile
    {
        public PollDTOMapperProfile()
        {
            CreateMap<CreatePollRequest, PollModel>();
        }
    }
}
