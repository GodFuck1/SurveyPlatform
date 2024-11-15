using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyPlatform.Core.Entities;

public class Poll
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public int AuthorID { get; set; }
    public User Author { get; set; }
    public List<PollOption> Options { get; set; }
    public List<PollResponse> Responses { get; set; }
}
