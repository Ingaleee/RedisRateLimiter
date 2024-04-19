using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkApp.Dal.Entities
{
    public record TaskCommentGetModel
    {
        public required long TaskId
        {
            get; init;
        }
        public required bool IncludeDeleted
        {
            get; init;
        }
    }
}
