using HomeworkApp.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkApp.Dal.Repositories.Interfaces
{
    public interface ITaskCommentRepository
    {
        Task<long> Add(TaskCommentEntityV1 model, CancellationToken token);
        Task Update(TaskCommentEntityV1 model, CancellationToken token);
        Task SetDeleted(long taskId, CancellationToken token);
        Task<TaskCommentEntityV1[]> Get(TaskCommentGetModel model, CancellationToken token);
    }
}
