using AutoMapper;
using SurveyPlatform.BLL.Models;
using SurveyPlatform.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyPlatform.BLL.Mappings
{
    public class PollMapperProfile : Profile
    {
        public PollMapperProfile()
        {
            CreateMap<PollModel, Poll>();
        }
    }
}
