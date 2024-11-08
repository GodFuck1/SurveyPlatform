using System.ComponentModel.DataAnnotations;

namespace SurveyPlatform.Models.Responses
{
    public class ResponseResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedIn { get; set; }
        public List<Option> Options { get; set; }
        public int AuthorID { get; set; }

    }
}
