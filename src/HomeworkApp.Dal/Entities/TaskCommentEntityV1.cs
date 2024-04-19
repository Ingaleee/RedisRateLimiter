using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkApp.Dal.Entities
{
    public class TaskCommentEntityV1
    {
        public long Id
        {
            get; set;
        }
        public long TaskId
        {
            get; set;
        }
        public long AuthorUserId
        {
            get; set;
        } 
        public string Message
        {
            get; set;
        }
        public DateTimeOffset At
        {
            get; set;
        } 
        public DateTimeOffset? ModifiedAt
        {
            get; set;
        } 
        public DateTimeOffset? DeletedAt
        {
            get; set;
        }

        public TaskCommentEntityV1()
        {
        }

        public TaskCommentEntityV1(long taskId, long authorUserId, string message, DateTimeOffset at)
        {
            TaskId = taskId;
            AuthorUserId = authorUserId;
            Message = message;
            At = at;
        }
    }

}
