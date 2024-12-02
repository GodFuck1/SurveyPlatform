namespace SurveyPlatform.BLL.Models;

public class PollResponseModel
{
    public Guid Id { get; set; }
    public Guid OptionId { get; set; }
    public Guid UserId { get; set; }
}
