using SurveyPlatform.DAL.Entities;
namespace SurveyPlatform.BLL.Models;
public class UserResponses
{
    public Guid Id { get; set; }
    public ICollection<PollResponseModel>? Responses { get; set; }
}
