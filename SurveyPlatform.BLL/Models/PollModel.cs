using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyPlatform.BLL.Models
{
    public class PollModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<PollOptionModel> Options { get; set; }
        public ICollection<PollResponseModel> Responses { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid AuthorID { get; set; }
    }

    public class PollOptionModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
    }
    public class PollResponseModel
    {
        public Guid Id { get; set; }
        public Guid OptionId { get; set; }
        public Guid UserId { get; set; }
    }
}
