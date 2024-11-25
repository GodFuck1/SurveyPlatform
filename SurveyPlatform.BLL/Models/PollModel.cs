using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyPlatform.BLL.Models
{
    public class PollModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Options { get; set; }
        public Guid AuthorID { get; set; }
    }
}
