using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkApp.Dal.Entities
{
    public class TaskMessage
    {
        public long TaskId
        {
            get; set;
        }
        public string Comment
        {
            get; set;
        }
        public bool IsDeleted
        {
            get; set;
        } 
        public DateTimeOffset At
        {
            get; set;
        } 

        public TaskMessage()
        {
        }

        public TaskMessage(long taskId, string comment, bool isDeleted, DateTimeOffset at)
        {
            TaskId = taskId;
            Comment = comment;
            IsDeleted = isDeleted;
            At = at;
        }
    }

}
