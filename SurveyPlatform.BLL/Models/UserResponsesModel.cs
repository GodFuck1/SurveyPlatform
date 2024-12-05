namespace SurveyPlatform.BLL.Models;
public class UserResponsesModel
{
    public Guid Id { get; set; }
    public ICollection<PollResponseModel> Responses { get; set; } = [];
}
