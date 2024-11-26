using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyPlatform.BLL.Models
{
    public class SubmitResponseModel
    {
        public Guid OptionID { get; set; }
        public Guid UserID { get; set; }
    }
}
