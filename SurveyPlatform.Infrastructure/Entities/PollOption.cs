using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyPlatform.DAL.Entities
{
    public class PollOption
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PollId { get; set; }
        public Poll Poll { get; set; }
        public ICollection<PollResponse> Responses { get; set; } // Добавлено
    }
}
