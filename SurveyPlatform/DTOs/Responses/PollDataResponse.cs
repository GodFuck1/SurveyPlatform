﻿using System.ComponentModel.DataAnnotations;

namespace SurveyPlatform.DTOs.Responses
{
    public class PollDataResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedIn { get; set; }
        public int AuthorID { get; set; }
        public List<OptionResponse> Options { get; set; }
    }
    public class OptionResponse
    {
        public int Id { get; set; }
        public string Content { get; set; }
    }
}